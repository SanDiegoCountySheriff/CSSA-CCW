using CCW.Common.Enums;
using CCW.Common.Models;
using CCW.Payment.Services;
using GlobalPayments.Api;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.Entities.Billing;
using GlobalPayments.Api.Entities.Enums;
using GlobalPayments.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace CCW.Payment.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly string _merchantName;
    private readonly string _hmacKey;
    private readonly BillPayService _billPayService;
    private readonly string _heartlandEndpoint;
    private readonly string _processTransactionEndpoint;
    private readonly string _redirectEndpoint;

    public PaymentController(ILogger<PaymentController> logger, IConfiguration configuration, ICosmosDbService cosmosDbService)
    {
        _logger = logger;
        _cosmosDbService = cosmosDbService;
        _merchantName = configuration.GetSection("Heartland:MerchantName").Value;
        _hmacKey = configuration.GetSection("Heartland:HmacKey").Value;
        _heartlandEndpoint = configuration.GetSection("Heartland").GetSection("HeartlandEndpoint").Value;
        _processTransactionEndpoint = configuration.GetSection("Heartland").GetSection("ProcessTransactionEndpoint").Value;
        _redirectEndpoint = configuration.GetSection("Heartland").GetSection("RedirectEndpoint").Value;

        ServicesContainer.ConfigureService(new BillPayConfig()
        {
            Username = configuration.GetSection("Heartland:Username").Value,
            Password = configuration.GetSection("Heartland:Password").Value,
            MerchantName = _merchantName,
            ServiceUrl = configuration.GetSection("Heartland:ServiceUrl").Value,
        });

        _billPayService = new BillPayService();
    }

    [Route("processTransaction")]
    [HttpPost]
    public IActionResult ProcessTransaction([FromForm] TransactionResponse transactionResponse, string applicationId, string paymentType)
    {
        var responseEndpoint = GetResponseEndpoint(paymentType);

        var parameters = new Dictionary<string, string>()
        {
            { "transactionId", transactionResponse.TransactionID },
            { "successful", transactionResponse.Successful },
            { "amount", transactionResponse.BaseAmount.ToString() },
            { "transactionDateTime", transactionResponse.TransactionDateTime },
            { "paymentType", paymentType }
        };

        var url = $"{_redirectEndpoint}{responseEndpoint}?applicationId={applicationId}&isComplete=true";
        var parameterizedUrl = AddHmacParamsToUrl(url, _hmacKey, parameters);

        return new RedirectResult(parameterizedUrl);
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("requestRefund")]
    [HttpPost]
    public async Task<IActionResult> RequestRefund(RefundRequest refundRequest)
    {
        try
        {
            await _cosmosDbService.AddRefundRequest(refundRequest);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError("There was a problem requesting a refund", ex.Message);
            return NotFound("There was a problem requesting a refund.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("getRefundRequests")]
    [HttpGet]
    public async Task<IActionResult> GetRefundRequests()
    {
        try
        {
            var result = await _cosmosDbService.GetAllRefundRequests();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"There was a problem getting refund requests: {ex.Message}");
            return NotFound("There was a problem getting refund requests");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("updatePaymentHistory")]
    [HttpPost]
    public async Task<IActionResult> UpdatePaymentHistory(
        string transactionId,
        bool successful,
        decimal amount,
        Common.Enums.PaymentType paymentType,
        string transactionDateTime,
        string hmac,
        string applicationId
    )
    {
        var parameters = new Dictionary<string, string>()
        {
            { "transactionId", transactionId },
            { "successful", successful.ToString().ToLower() },
            { "amount", amount.ToString() },
            { "transactionDateTime", transactionDateTime },
            { "paymentType", paymentType.ToString() }
        };

        string queryString = string.Join("&", parameters
            .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

        string verificationHmac = GenerateHmac(_hmacKey, queryString);

        if (verificationHmac != hmac)
        {
            return new BadRequestResult();
        }

        try
        {
            GetUserId(out string userId);

            var application = await _cosmosDbService.GetApplication(applicationId, userId);
            var paymentHistory = new PaymentHistory();

            var failedPaymentHistory = application.PaymentHistory.Where(ph => ph.Successful == false && ph.PaymentType == paymentType).FirstOrDefault();
            var duplicatePaymentHistory = application.PaymentHistory.Where(ph => ph.PaymentType == paymentType && ph.TransactionId == transactionId).FirstOrDefault();

            if (duplicatePaymentHistory != null)
            {
                return new UnprocessableEntityResult();
            }

            if (failedPaymentHistory != null)
            {
                application.PaymentHistory.Remove(failedPaymentHistory);
            }

            if (paymentType is Common.Enums.PaymentType.InitialStandard or Common.Enums.PaymentType.InitialJudicial or Common.Enums.PaymentType.InitialReserve or Common.Enums.PaymentType.InitialEmployment)
            {
                var initialAmount = paymentType switch
                {
                    Common.Enums.PaymentType.InitialStandard => decimal.Parse(application.Application.Cost.New.Standard.ToString()),
                    Common.Enums.PaymentType.InitialJudicial => decimal.Parse(application.Application.Cost.New.Judicial.ToString()),
                    Common.Enums.PaymentType.InitialReserve => decimal.Parse(application.Application.Cost.New.Reserve.ToString()),
                    Common.Enums.PaymentType.InitialEmployment => decimal.Parse(application.Application.Cost.New.Employment.ToString()),
                    _ => decimal.Parse(application.Application.Cost.New.Standard.ToString())
                };

                var livescanAmount = paymentType switch
                {
                    Common.Enums.PaymentType.InitialStandard => decimal.Parse(application.Application.Cost.StandardLivescanFee.ToString()),
                    Common.Enums.PaymentType.InitialJudicial => decimal.Parse(application.Application.Cost.JudicialLivescanFee.ToString()),
                    Common.Enums.PaymentType.InitialReserve => decimal.Parse(application.Application.Cost.ReserveLivescanFee.ToString()),
                    Common.Enums.PaymentType.InitialEmployment => decimal.Parse(application.Application.Cost.EmploymentLivescanFee.ToString()),
                    _ => decimal.Parse(application.Application.Cost.StandardLivescanFee.ToString())
                };

                var livescanPaymentType = paymentType switch
                {
                    Common.Enums.PaymentType.InitialStandard => Common.Enums.PaymentType.StandardLivescan,
                    Common.Enums.PaymentType.InitialJudicial => Common.Enums.PaymentType.JudicialLivescan,
                    Common.Enums.PaymentType.InitialReserve => Common.Enums.PaymentType.ReserveLivescan,
                    Common.Enums.PaymentType.InitialEmployment => Common.Enums.PaymentType.EmploymentLivescan,
                    _ => Common.Enums.PaymentType.StandardLivescan
                };

                var initialPaymentHistory = application.PaymentHistory.Where(ph =>
                {
                    return ph.PaymentType == paymentType && ph.Amount == initialAmount;
                }).FirstOrDefault();

                application.PaymentHistory.Remove(initialPaymentHistory);

                var livescanPaymentHistory = application.PaymentHistory.Where(ph =>
                {
                    return ph.PaymentType == livescanPaymentType && ph.Amount == livescanAmount;
                }).FirstOrDefault();

                application.PaymentHistory.Remove(livescanPaymentHistory);

                initialPaymentHistory.Verified = true;
                initialPaymentHistory.TransactionId = transactionId;
                initialPaymentHistory.Successful = successful;
                initialPaymentHistory.PaymentDateTimeUtc = DateTimeOffset.Parse(transactionDateTime).ToUniversalTime();

                livescanPaymentHistory.Verified = true;
                livescanPaymentHistory.TransactionId = transactionId;
                livescanPaymentHistory.Successful = successful;
                livescanPaymentHistory.PaymentDateTimeUtc = DateTimeOffset.Parse(transactionDateTime).ToUniversalTime();

                application.PaymentHistory.Add(initialPaymentHistory);
                application.PaymentHistory.Add(livescanPaymentHistory);
                application.Application.ReadyForInitialPayment = !successful;
            }
            else if (paymentType is Common.Enums.PaymentType.RenewalStandard or Common.Enums.PaymentType.RenewalJudicial or Common.Enums.PaymentType.RenewalReserve or Common.Enums.PaymentType.RenewalEmployment)
            {
                var renewalAmount = paymentType switch
                {
                    Common.Enums.PaymentType.RenewalStandard => decimal.Parse(application.Application.Cost.Renew.Standard.ToString()),
                    Common.Enums.PaymentType.RenewalJudicial => decimal.Parse(application.Application.Cost.Renew.Judicial.ToString()),
                    Common.Enums.PaymentType.RenewalReserve => decimal.Parse(application.Application.Cost.Renew.Reserve.ToString()),
                    Common.Enums.PaymentType.RenewalEmployment => decimal.Parse(application.Application.Cost.Renew.Employment.ToString()),
                    _ => decimal.Parse(application.Application.Cost.New.Standard.ToString())
                };

                var existingPaymentHistory = application.PaymentHistory.Where(ph =>
                {
                    return ph.PaymentType == paymentType && ph.Amount == renewalAmount;
                }).FirstOrDefault();

                application.PaymentHistory.Remove(existingPaymentHistory);

                existingPaymentHistory.Verified = true;
                existingPaymentHistory.TransactionId = transactionId;
                existingPaymentHistory.Successful = successful;
                existingPaymentHistory.PaymentDateTimeUtc = DateTimeOffset.Parse(transactionDateTime).ToUniversalTime();

                application.PaymentHistory.Add(existingPaymentHistory);
                application.Application.ReadyForRenewalPayment = !successful;
            }
            else if (paymentType is Common.Enums.PaymentType.ModificationStandard or Common.Enums.PaymentType.ModificationJudicial or Common.Enums.PaymentType.ModificationReserve or Common.Enums.PaymentType.ModificationEmployment)
            {
                var existingPaymentHistory = application.PaymentHistory.Where(ph =>
                {
                    return ph.PaymentType == paymentType && ph.Amount == decimal.Parse(application.Application.Cost.Modify.ToString()) && ph.ModificationNumber == application.Application.ModificationNumber;
                }).FirstOrDefault();

                application.PaymentHistory.Remove(existingPaymentHistory);

                existingPaymentHistory.Verified = true;
                existingPaymentHistory.TransactionId = transactionId;
                existingPaymentHistory.Successful = successful;
                existingPaymentHistory.PaymentDateTimeUtc = DateTimeOffset.Parse(transactionDateTime).ToUniversalTime();

                application.PaymentHistory.Add(existingPaymentHistory);
                application.Application.ReadyForModificationPayment = !successful;
            }

            application.Application.PaymentStatus = PaymentStatus.OnlineSubmitted;
            await _cosmosDbService.UpdateApplication(application);

            return new OkObjectResult(successful);
        }
        catch (Exception ex)
        {
            _logger.LogError("There was a problem processing the transaction", ex.Message);
            return new NotFoundObjectResult(false);
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("makePayment")]
    [HttpGet]
    public async Task<IActionResult> MakePayment(string applicationId, decimal amount, decimal livescanAmount, string orderId, Common.Enums.PaymentType paymentType)
    {
        try
        {
            GetUserId(out string userId);
            var application = await _cosmosDbService.GetApplication(applicationId, userId);

            application.PaymentHistory ??= new List<PaymentHistory>();

            var paymentTypeDescription = GetEnumDescription(paymentType);
            var total = amount;

            if (livescanAmount > 0)
            {
                total += livescanAmount;
            }

            var bill = new Bill()
            {
                Amount = total,
                BillType = paymentTypeDescription,
                Identifier1 = orderId,
                // first name
                Identifier2 = "",
                // last name
                Identifier3 = "",
                Identifier4 = DateTime.Now.ToString(),
            };

            HostedPaymentData hostedPaymentData = new()
            {
                Bills = new List<Bill>() { bill },
                CaptureAddress = false,
                CustomerAddress = new GlobalPayments.Api.Entities.Address(),
                CustomerEmail = "",
                CustomerFirstName = "",
                CustomerLastName = "",
                HostedPaymentType = HostedPaymentType.MakePayment,
                MerchantResponseUrl = $"{_processTransactionEndpoint}?applicationId={applicationId}&paymentType={paymentType}",
                CustomerIsEditable = true,
            };

            var paymentHistory = new PaymentHistory()
            {
                PaymentDateTimeUtc = DateTimeOffset.UtcNow,
                Amount = amount,
                VendorInfo = "Credit Card",
                PaymentType = paymentType,
                PaymentStatus = PaymentStatus.OnlineSubmitted,
                Verified = false,
                Successful = true
            };

            if (paymentType is Common.Enums.PaymentType.ModificationStandard or Common.Enums.PaymentType.ModificationJudicial or Common.Enums.PaymentType.ModificationReserve or Common.Enums.PaymentType.ModificationEmployment)
            {
                paymentHistory.ModificationNumber = application.Application.ModificationNumber;
            }

            application.PaymentHistory.Add(paymentHistory);

            if (livescanAmount > 0)
            {
                var livescanPaymentType = paymentType switch
                {
                    Common.Enums.PaymentType.InitialStandard => Common.Enums.PaymentType.StandardLivescan,
                    Common.Enums.PaymentType.InitialJudicial => Common.Enums.PaymentType.JudicialLivescan,
                    Common.Enums.PaymentType.InitialReserve => Common.Enums.PaymentType.ReserveLivescan,
                    Common.Enums.PaymentType.InitialEmployment => Common.Enums.PaymentType.EmploymentLivescan,
                    _ => Common.Enums.PaymentType.StandardLivescan
                };

                var livescanPaymentHistory = new PaymentHistory()
                {
                    PaymentDateTimeUtc = DateTimeOffset.UtcNow,
                    Amount = livescanAmount,
                    VendorInfo = "Credit Card",
                    PaymentType = livescanPaymentType,
                    PaymentStatus = PaymentStatus.OnlineSubmitted,
                    Verified = false,
                    Successful = true
                };

                application.PaymentHistory.Add(livescanPaymentHistory);
            }

            await _cosmosDbService.UpdateApplication(application);

            var response = _billPayService.LoadHostedPayment(hostedPaymentData);

            return Ok($"{_heartlandEndpoint}/webpayments/{_merchantName}/GUID/{response.PaymentIdentifier}");
        }
        catch (Exception ex)
        {
            _logger.LogError("There was a problem making the payment.", ex.Message);
            return new BadRequestResult();
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("refundPayment")]
    [HttpPost]
    public async Task<IActionResult> RefundPayment([FromBody] RefundRequest refundRequest, decimal convenienceFee)
    {
        PermitApplication application;
        PaymentHistory paymentHistory;

        try
        {
            if (refundRequest.Id != null)
            {
                var refundRequestResult = await _cosmosDbService.GetRefundRequest(refundRequest.Id);

                if (refundRequestResult != null)
                {
                    await _cosmosDbService.DeleteRefundRequest(refundRequest);
                }
            }

            application = await _cosmosDbService.GetAdminApplication(refundRequest.ApplicationId);
            paymentHistory = application.PaymentHistory.Where(ph => ph.TransactionId == refundRequest.TransactionId).FirstOrDefault();
            paymentHistory.RefundAmount += refundRequest.RefundAmount;

            await _cosmosDbService.UpdateApplication(application);
        }
        catch (Exception ex)
        {
            _logger.LogError("There was a problem updating the application while refunding", ex.Message);

            return new BadRequestResult();
        }

        try
        {
            if (paymentHistory.PaymentStatus == PaymentStatus.InPerson)
            {
                return Ok();
            }

            decimal fee = refundRequest.RefundAmount * convenienceFee;

            Transaction.FromId(refundRequest.TransactionId).Refund(refundRequest.RefundAmount).WithCurrency("USD").WithConvenienceAmount(fee).Execute();

            return Ok();
        }
        catch (GatewayException ex)
        {
            _logger.LogError("There was a gateway exception within GlobalPayments", ex.ResponseMessage);
            paymentHistory.RefundAmount -= refundRequest.RefundAmount;

            await _cosmosDbService.UpdateApplication(application);

            return new BadRequestResult();
        }
        catch (Exception ex)
        {
            _logger.LogError("There was a problem issuing the refund from Heartland", ex.Message);
            paymentHistory.RefundAmount -= refundRequest.RefundAmount;

            await _cosmosDbService.UpdateApplication(application);

            return new BadRequestResult();
        }
    }

    public class TransactionResponse
    {
        public string Successful { get; set; }
        public string TransactionID { get; set; }
        public string TransactionDateTime { get; set; }
        public decimal BaseAmount { get; set; }
    }

    private void GetUserId(out string userId)
    {
        userId = HttpContext.User.Claims
            .Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
            .Select(c => c.Value).FirstOrDefault();

        if (userId == null)
        {
            throw new ArgumentNullException("userId", "Invalid token.");
        }
    }

    static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

        return attribute == null ? value.ToString() : attribute.Description;
    }

    static string GetResponseEndpoint(string paymentType)
    {
        if (paymentType is "InitialEmployment" or "InitialJudicial" or "InitialReserve" or "InitialStandard")
        {
            return "application-details";
        }
        if (paymentType is "RenewalEmployment" or "RenewalJudicial" or "RenewalReserve" or "RenewalStandard")
        {
            return "application-details";
        }
        else
        {
            return "application-details";
        }
    }

    private static string AddHmacParamsToUrl(string url, string key, Dictionary<string, string> parameters)
    {
        string queryString = string.Join("&", parameters
            .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

        string hmac = GenerateHmac(key, queryString);
        string signedUrl = $"{url}&{queryString}&hmac={Uri.EscapeDataString(hmac)}";

        return signedUrl;
    }

    private static string GenerateHmac(string key, string data)
    {
        using (var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            byte[] hashBytes = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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

    public PaymentController(ILogger<PaymentController> logger, IConfiguration configuration, ICosmosDbService cosmosDbService)
    {
        _logger = logger;
        _cosmosDbService = cosmosDbService;
        var client = new SecretClient(new Uri(configuration.GetSection("KeyVault:VaultUri").Value), credential: new DefaultAzureCredential());
        _merchantName = client.GetSecret("heartland-merchant-name").Value.Value;
        _hmacKey = client.GetSecret("hmac-key").Value.Value;

        ServicesContainer.ConfigureService(new BillPayConfig()
        {
            Username = client.GetSecret("heartland-username").Value.Value,
            Password = client.GetSecret("heartland-password").Value.Value,
            MerchantName = _merchantName,
            ServiceUrl = client.GetSecret("heartland-service-url").Value.Value,
        });

        _billPayService = new BillPayService();
    }

    // TODO: figure out what to do with expired cards, etc.
    [Route("processTransaction")]
    [HttpPost]
    public IActionResult ProcessTransaction([FromForm] TransactionResponse transactionResponse, string applicationId, string paymentType)
    {
        var parameters = new Dictionary<string, string>()
        {
            { "transactionId", transactionResponse.TransactionID },
            { "successful", transactionResponse.Successful },
            { "amount", transactionResponse.BaseAmount.ToString() },
            { "transactionDateTime", transactionResponse.TransactionDateTime },
            { "paymentType", paymentType }
        };

        // TODO: make endpoint application setting, maybe window.location.origin from front end?
        var url = $"http://localhost:4000/finalize?applicationId={applicationId}&isComplete=false";
        var parameterizedUrl = AddHmacParamsToUrl(url, _hmacKey, parameters);

        return new RedirectResult(parameterizedUrl);
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
            var duplicatePaymentHistory = application.PaymentHistory.Where(ph => ph.PaymentType == paymentType).FirstOrDefault();

            if (duplicatePaymentHistory != null)
            {
                return new UnprocessableEntityResult();
            }

            if (failedPaymentHistory != null)
            {
                application.PaymentHistory.Remove(failedPaymentHistory);
            }

            if (successful)
            {
                paymentHistory.TransactionId = transactionId;
                paymentHistory.PaymentDateTimeUtc = DateTime.Parse(transactionDateTime).ToUniversalTime();
                paymentHistory.Amount = amount;
                paymentHistory.VendorInfo = "Credit Card";
                paymentHistory.PaymentType = paymentType;
                paymentHistory.Successful = true;
                paymentHistory.PaymentStatus = PaymentStatus.OnlineSubmitted;
            }
            else
            {
                paymentHistory.TransactionId = transactionId;
                paymentHistory.PaymentDateTimeUtc = DateTime.Parse(transactionDateTime).ToUniversalTime();
                paymentHistory.Amount = amount;
                paymentHistory.VendorInfo = "Credit Card";
                paymentHistory.PaymentType = paymentType;
                paymentHistory.Successful = false;
                paymentHistory.PaymentStatus = PaymentStatus.OnlineSubmitted;
            }

            application.PaymentHistory.Add(paymentHistory);
            application.Application.PaymentStatus = PaymentStatus.OnlineSubmitted;
            await _cosmosDbService.UpdateApplication(application);

            return new OkObjectResult(true);
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
    public async Task<IActionResult> MakePayment(string applicationId, decimal amount, string orderId, Common.Enums.PaymentType paymentType)
    {
        try
        {
            GetUserId(out string userId);
            var paymentTypeDescription = GetEnumDescription(paymentType);

            var bill = new Bill()
            {
                Amount = amount,
                BillType = paymentTypeDescription,
                Identifier1 = orderId,
                // first name
                Identifier2 = "",
                // last name
                Identifier3 = "",
                Identifier4 = DateTime.Now.ToString(),
            };

            var response = _billPayService.LoadHostedPayment(new HostedPaymentData()
            {
                Bills = new List<Bill>() { bill },
                CaptureAddress = false,
                CustomerAddress = new GlobalPayments.Api.Entities.Address(),
                CustomerEmail = "",
                CustomerFirstName = "",
                CustomerLastName = "",
                HostedPaymentType = HostedPaymentType.MakePayment,
                // TODO: make endpoint application setting
                MerchantResponseUrl = $"http://localhost:5180/payment/v1/payment/processTransaction?applicationId={applicationId}&paymentType={paymentType}",
                CustomerIsEditable = true,
            });

            // TODO: make endpoint application setting
            return Ok($"https://staging.heartlandpaymentservices.net/webpayments/{_merchantName}/GUID/{response.PaymentIdentifier}");
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
    public async Task<IActionResult> RefundPayment([FromBody] RefundRequest refundRequest)
    {
        PermitApplication application;
        PaymentHistory paymentHistory;

        try
        {
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

            Transaction.FromId(refundRequest.TransactionId).Refund(refundRequest.RefundAmount).WithCurrency("USD").Execute();

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

    public class RefundRequest
    {
        public string TransactionId { get; set; }
        public string ApplicationId { get; set; }
        public decimal RefundAmount { get; set; }
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

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Common.Models;
using CCW.Common.Enums;
using CCW.Payment.Services;
using GlobalPayments.Api;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.Entities.Billing;
using GlobalPayments.Api.Entities.Enums;
using GlobalPayments.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using GlobalPayments.Api.Builders;

namespace CCW.Payment.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly string _merchantName;

    public PaymentController(ILogger<PaymentController> logger, IConfiguration configuration, ICosmosDbService cosmosDbService)
    {
        _logger = logger;
        _cosmosDbService = cosmosDbService;
        var client = new SecretClient(new Uri(configuration.GetSection("KeyVault:VaultUri").Value), credential: new DefaultAzureCredential());
        _merchantName = client.GetSecret("heartland-merchant-name").Value.Value;

        ServicesContainer.ConfigureService(new BillPayConfig()
        {
            Username = client.GetSecret("heartland-username").Value.Value,
            Password = client.GetSecret("heartland-password").Value.Value,
            MerchantName = _merchantName,
            ServiceUrl = client.GetSecret("heartland-service-url").Value.Value,
        });
    }

    // TODO: figure out authentication for this endpoint
    [Route("processTransaction")]
    [HttpPost]
    public async Task<IActionResult> ProcessTransaction([FromForm] TransactionResponse transactionResponse, string applicationId, string paymentType, string userId)
    {
        var application = await _cosmosDbService.GetApplication(applicationId, userId);
        var paymentHistory = new PaymentHistory();

        var failedPaymentHistory = application.PaymentHistory.Where(ph => ph.Successful == false && ph.PaymentType == paymentType).FirstOrDefault();

        if (failedPaymentHistory != null)
        {
            application.PaymentHistory.Remove(failedPaymentHistory);
        }

        if (transactionResponse.Successful == "true")
        {
            paymentHistory.TransactionId = transactionResponse.TransactionID;
            paymentHistory.PaymentDateTimeUtc = DateTime.Parse(transactionResponse.TransactionDateTime).ToUniversalTime();
            paymentHistory.Amount = transactionResponse.BaseAmount;
            paymentHistory.VendorInfo = "Credit Card";
            paymentHistory.PaymentType = paymentType;
            paymentHistory.Successful = true;
            paymentHistory.PaymentStatus = PaymentStatus.OnlineSubmitted;
        }
        else
        {
            paymentHistory.TransactionId = transactionResponse.TransactionID;
            paymentHistory.PaymentDateTimeUtc = DateTime.Parse(transactionResponse.TransactionDateTime).ToUniversalTime();
            paymentHistory.Amount = transactionResponse.BaseAmount;
            paymentHistory.VendorInfo = "Credit Card";
            paymentHistory.PaymentType = paymentType;
            paymentHistory.Successful = false;
            paymentHistory.PaymentStatus = PaymentStatus.OnlineSubmitted;
        }

        application.PaymentHistory.Add(paymentHistory);
        await _cosmosDbService.UpdateApplication(application);

        return new RedirectResult($"http://localhost:4000/finalize?applicationId={applicationId}&isComplete=false");
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("makePayment")]
    [HttpGet]
    public IActionResult MakePayment(string applicationId, decimal amount, string orderId, Common.Enums.PaymentType paymentType)
    {
        GetUserId(out string userId);

        var billPayService = new BillPayService();

        var address = new GlobalPayments.Api.Entities.Address()
        {
        };

        var bill = new Bill()
        {
            Amount = amount,
            BillType = GetEnumDescription(paymentType),
            // Order ID
            Identifier1 = orderId,
            // first name
            Identifier2 = "",
            // last name
            Identifier3 = "",
            // date?
            Identifier4 = DateTime.Now.ToString(),
        };

        var response = billPayService.LoadHostedPayment(new HostedPaymentData()
        {
            Bills = new List<Bill>() { bill },
            CaptureAddress = false,
            CustomerAddress = address,
            CustomerEmail = "",
            CustomerFirstName = "",
            CustomerLastName = "",
            HostedPaymentType = HostedPaymentType.MakePayment,
            MerchantResponseUrl = $"http://localhost:5180/payment/v1/payment/processTransaction?applicationId={applicationId}&paymentType={bill.BillType}&userId={userId}",
            CustomerIsEditable = true,
        });

        return Ok($"https://staging.heartlandpaymentservices.net/webpayments/{_merchantName}/GUID/{response.PaymentIdentifier}");
    }

    // TODO: figure out authentication for this endpoint
    [Route("refundPayment")]
    [HttpPost]
    public async Task<IActionResult> RefundPayment([FromBody] RefundRequest refundRequest)
    {
        try
        {
           var items = ReportingService.FindTransactions()
                   .Where(SearchCriteria.)

                   .Execute();
            var transaction = Transaction.FromId(refundRequest.TransactionId).Refund(refundRequest.RefundAmount).WithCurrency("USD").Execute();

            // Transaction test = Transaction.FromClientTransactionId(refundRequest.TransactionId, PaymentMethodType.Credit);

            // Transaction result = test.Refund(refundRequest.RefundAmount).WithCurrency("USD").Execute();

            // ManagementBuilder mb = Transaction.FromId(refundRequest.TransactionId).Refund(refundRequest.RefundAmount).WithCurrency("USD");

            Console.WriteLine(transaction);

            // TODO: update application

            return Ok();
        }
        catch (GatewayException ex) {
            Console.WriteLine("There was a gateway exception within GlobalPayments", ex.Message);
            return new BadRequestResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error processing refund", ex.Message);
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
}

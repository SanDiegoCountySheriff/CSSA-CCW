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

    [Route("processTransaction")]
    [HttpPost]
    public async Task<IActionResult> ProcessTransaction([FromForm] TransactionResponse transactionResponse, string applicationId, string paymentType, string userId)
    {
        if (transactionResponse.Successful == "true")
        {
            var application = await _cosmosDbService.GetApplication(applicationId, userId);

            var paymentHistory = new PaymentHistory()
            {
                TransactionId = transactionResponse.TransactionID,
                PaymentDateTimeUtc = DateTime.Parse(transactionResponse.TransactionDateTime),
                Amount = transactionResponse.BaseAmount,
                VendorInfo = "Credit Card",
                PaymentType = paymentType,
            };

            application.PaymentHistory.Add(paymentHistory);

            await _cosmosDbService.UpdateApplication(application);
        }

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
            Identifier4 = DateTime.Now.ToString()
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
}

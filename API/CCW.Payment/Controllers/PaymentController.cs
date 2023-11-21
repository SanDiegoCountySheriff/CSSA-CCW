using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GlobalPayments.Api;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.Entities.Billing;
using GlobalPayments.Api.Entities.Enums;
using GlobalPayments.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Payment.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(ILogger<PaymentController> logger, IConfiguration configuration)
    {
        _logger = logger;
        var client = new SecretClient(new Uri(configuration.GetSection("KeyVault:VaultUri").Value), credential: new DefaultAzureCredential());

        ServicesContainer.ConfigureService(new BillPayConfig()
        {
            Username = client.GetSecret("heartland-username").Value.Value,
            Password = client.GetSecret("heartland-password").Value.Value,
            MerchantName = client.GetSecret("heartland-merchant-name").Value.Value,
            ServiceUrl = client.GetSecret("heartland-service-url").Value.Value,
        });
    }

    [Route("processTransaction")]
    [HttpPost]
    public async Task<IActionResult> ProcessTransaction([FromForm] TransactionResponse transactionResponse)
    {
        Console.WriteLine(transactionResponse);

        return new RedirectResult("http://localhost:4000/finalize?applicationId=9b907206-c829-4059-bd97-3f936fceb11f&isComplete=false");
    }

    [Route("makePayment")]
    [HttpGet]
    public async Task<IActionResult> MakePayment()
    {
        var billPayService = new BillPayService();

        var address = new Address()
        {
        };

        var bill = new Bill()
        {
            Amount = 50M,
            BillType = "CCW Application Initial Payment",
            Identifier1 = "ID 1",
            Identifier2 = "ID 2",
            Identifier3 = "ID 3",
            Identifier4 = "ID 4"
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
            MerchantResponseUrl = "http://localhost:5180/payment/v1/payment/processTransaction",
            CustomerIsEditable = true,
        });

        return Ok($"https://staging.heartlandpaymentservices.net/webpayments/SanDiegoSheriffPayment_Test/GUID/{response.PaymentIdentifier}");
    }

    public class TransactionResponse
    {
        public string Successful { get; set; }
        public string TransactionID { get; set; }
        public string TransactionDateTime { get; set; }
    }
}

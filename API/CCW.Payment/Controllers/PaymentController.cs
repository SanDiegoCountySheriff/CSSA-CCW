using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CCW.Payment.Services;
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
    private readonly ICosmosDbService _cosmosDbService;

    public PaymentController(ILogger<PaymentController> logger, IConfiguration configuration, ICosmosDbService cosmosDbService)
    {
        _logger = logger;
        _cosmosDbService = cosmosDbService;
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
    public IActionResult ProcessTransaction([FromForm] TransactionResponse transactionResponse)
    {
        Console.WriteLine(transactionResponse);

        return new RedirectResult("http://localhost:4000/finalize?applicationId=9b907206-c829-4059-bd97-3f936fceb11f&isComplete=false");
    }

    [Route("makePayment")]
    [HttpGet]
    public IActionResult MakePayment()
    {
        GetUserId(out string userId);

        var billPayService = new BillPayService();

        var address = new Address()
        {
        };

        var bill = new Bill()
        {
            Amount = 50M,
            BillType = "CCW Application Initial Payment",
            // application ID
            Identifier1 = "ID 1",
            // first name
            Identifier2 = "ID 2",
            // last name
            Identifier3 = "ID 3",
            // date?
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
        public string ApplicationId { get; set; }
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
}

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

    public PaymentController(ILogger<PaymentController> logger)
    {
        _logger = logger;
        ServicesContainer.ConfigureService(new BillPayConfig()
        {
            ServiceUrl = "https://staging.heartlandpaymentservices.net/"
        });
    }

    [Route("processTransaction")]
    [HttpPost]
    public async Task<IActionResult> ProcessTransaction([FromForm] object transaction) 
    {
        Console.WriteLine(transaction.ToString());

        return new RedirectResult("http://localhost:3000");
    }

    [Route("makePayment")]
    [HttpGet]
    public async Task<IActionResult> MakePayment()
    {
        var billPayService = new BillPayService();

        var address = new Address()
        {
            StreetAddress1 = "1234 Test St",
            StreetAddress2 = "Apt 201",
            City = "Auburn",
            State = "AL",
            Country = "US",
            PostalCode = "12345"
        };
        var customer = new Customer()
        {
            Address = address,
            Email = "testemailaddress@e-hps.com",
            FirstName = "Test",
            LastName = "Tester",
            HomePhone = "555-555-4444",
            Company = "Test Company",
            MiddleName = "Testing",
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

        var blindBill = new Bill()
        {
            Amount = 50M,
            BillType = "CCW Application Initial Payment",
            Identifier1 = "12345",
            Identifier2 = "23456",
            BillPresentment = BillPresentment.Full,
            DueDate = DateTime.Now,
            Customer = customer
        };

        var response = billPayService.LoadHostedPayment(new HostedPaymentData()
        {
            Bills = new List<Bill>() { bill },
            CaptureAddress = false,
            CustomerAddress = address,
            CustomerEmail = "test@tester.com",
            CustomerFirstName = "Test",
            CustomerLastName = "Tester",
            HostedPaymentType = HostedPaymentType.MakePayment,
            MerchantResponseUrl = "http://localhost:5180/payment/v1/payment/processTransaction",
        });

        return Ok($"https://staging.heartlandpaymentservices.net/webpayments/SanDiegoSheriffPayment_Test/GUID/{response.PaymentIdentifier}");
    }
}

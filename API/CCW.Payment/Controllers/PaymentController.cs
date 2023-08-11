using CCW.Common.Models;
using CCW.Payment.Clients;
using CCW.Payment.Services;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.Entities.Billing;
using GlobalPayments.Api.Entities.Enums;
using GlobalPayments.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Payment.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IHeartlandService _heartlandService;
    private readonly ILogger<PaymentController> _logger;
    private readonly IApplicationServiceClient _applicationServiceClient;

    public PaymentController(ILogger<PaymentController> logger, IHeartlandService heartlandService, IApplicationServiceClient applicationServiceClient)
    {
        _heartlandService = heartlandService;
        _logger = logger;
        _applicationServiceClient = applicationServiceClient;
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("chargeCard")]
    [HttpPut]
    public async Task<IActionResult> ChargeCard(string token, decimal amount, string applicationId)
    {
        try
        {
            GetUserId(out string userId);
            var response = _heartlandService.ChargeCard(token, amount);

            var paymentHistory = new PaymentHistory()
            {
                PaymentDateTimeUtc = DateTime.UtcNow,
                PaymentType = Common.Models.PaymentType.Online,
                Amount = amount,
                TransactionId = response.TransactionId,
                VendorInfo = "Heartland",
            };

            await _applicationServiceClient.UpdateApplicationPaymentHistoryAsync(paymentHistory, applicationId, userId, cancellationToken: default);

            return Ok(response);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve user permit application.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("refundPayment")]
    [HttpPut]
    public async Task<IActionResult> RefundPayment(string transactionId, decimal amount, string applicationId)
    {
        try
        {
            var response = _heartlandService.RefundPayment(transactionId, amount);

            var paymentHistory = new PaymentHistory()
            {
                PaymentDateTimeUtc = DateTime.UtcNow,
                PaymentType = Common.Models.PaymentType.Refund,
                Amount = amount,
                TransactionId = response.TransactionId,
                VendorInfo = "Heartland",
            };

            await _applicationServiceClient.UpdateUserApplicationPaymentHistoryAsync(paymentHistory, applicationId, cancellationToken: default);

            return Ok(response);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve user permit application.");
        }
    }

    [Authorize(Policy ="AADUsers")]
    [Route("loadHostedPayment")]
    [HttpGet]
    public async Task<IActionResult> LoadHostedPayment()
    {
        try
        {
            var service = new BillPayService();
            var bill = new Bill();
            var hostedPaymentData = new HostedPaymentData()
            {
                Bills = new List<Bill>() { bill },
                CustomerAddress = new Address { StreetAddress1 = "123 Drive", PostalCode = "12345" },
                CustomerEmail = "test@tester.com",
                CustomerFirstName = "Test",
                CustomerLastName = "Tester",
                HostedPaymentType = HostedPaymentType.MakePayment
            };
            var response = service.LoadHostedPayment(hostedPaymentData);

            return Ok(response);
        }
        catch (Exception ex)
        {
            var originalException = ex.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve user permit application.");
        }
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

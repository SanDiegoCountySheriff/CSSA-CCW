using CCW.Payment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Payment.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IHeartlandService _heartlandService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(ILogger<PaymentController> logger, IHeartlandService heartlandService)
    {
        _heartlandService = heartlandService;
        _logger = logger;
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("chargeCard")]
    [HttpPut]
    public async Task<IActionResult> ChargeCard(string token, decimal amount, string applicationId, string paymentType)
    {
        try
        {
            GetUserId(out string userId);
            var response = await _heartlandService.ChargeCard(token, amount, applicationId, userId, paymentType, cancellationToken: default);

            return Ok(response);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve user permit application.");
        }
    }

    [Authorize(Policy = "RequireSystemAdminOnly")]
    [Route("refundPayment")]
    [HttpPut]
    public async Task<IActionResult> RefundPayment(string transactionId, decimal amount, string applicationId)
    {
        try
        {
            var response = await _heartlandService.RefundPayment(transactionId, amount, applicationId, cancellationToken: default);

            return Ok(response);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
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

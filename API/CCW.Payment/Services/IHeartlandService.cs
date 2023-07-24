using GlobalPayments.Api.Entities;

namespace CCW.Payment.Services;

public interface IHeartlandService
{
    Transaction ChargeCard(string token, decimal amount);
    Transaction RefundPayment(string transactionId, decimal amount);
}

using GlobalPayments.Api.Entities;

namespace CCW.Payment.Services;

public interface IHeartlandService
{
    Task<Transaction> ChargeCard(string token, decimal amount, string applicationId, string userId, string paymentType, CancellationToken cancellationToken);
    Task<Transaction> RefundPayment(string transactionId, decimal amount, string applicationId, CancellationToken cancellationToken);
}

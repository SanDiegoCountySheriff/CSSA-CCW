using Azure.Security.KeyVault.Secrets;
using CCW.Common.Models;
using CCW.Payment.Clients;
using GlobalPayments.Api;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.PaymentMethods;

namespace CCW.Payment.Services;

public class HeartlandService : IHeartlandService
{
    private readonly string _apiKey;
    private readonly SecretClient _client;
    private readonly IApplicationServiceClient _applicationServiceClient;

    public HeartlandService(SecretClient client, IApplicationServiceClient applicationServiceClient)
    {
        _client = client;
        _apiKey = _client.GetSecret("HeartlandSandboxAPISecretKey").Value.Value;
        _applicationServiceClient = applicationServiceClient;

        ServicesContainer.ConfigureService(new PorticoConfig
        {
            SecretApiKey = _apiKey,
            DeveloperId = "000000",
            VersionNumber = "0000",
            ServiceUrl = "https://cert.api2.heartlandportico.com"
        });
    }

    public async Task<Transaction> ChargeCard(string token, decimal amount, string applicationId, string userId, string paymentType, CancellationToken cancellationToken)
    {
        var card = new CreditCardData
        {
            Token = token
        };

        var response = card.Charge(amount)
            .WithCurrency("USD")
            .Execute();

        var paymentHistory = new PaymentHistory()
        {
            PaymentDateTimeUtc = DateTime.UtcNow,
            PaymentType = paymentType,
            Amount = amount,
            TransactionId = response.TransactionId,
            VendorInfo = "Heartland",
        };

        await _applicationServiceClient.UpdateApplicationPaymentHistoryAsync(paymentHistory, applicationId, userId, cancellationToken);

        return response;
    }

    public async Task<Transaction> RefundPayment(string transactionId, decimal amount, string applicationId, CancellationToken cancellationToken)
    {
        var response = Transaction.FromId(transactionId).Refund(amount).WithCurrency("USD").Execute();

        var paymentHistory = new PaymentHistory()
        {
            PaymentDateTimeUtc = DateTime.UtcNow,
            PaymentType = "Refund",
            Amount = amount,
            TransactionId = response.TransactionId,
            VendorInfo = "Heartland",
        };

        await _applicationServiceClient.UpdateUserApplicationPaymentHistoryAsync(paymentHistory, applicationId, cancellationToken);

        return response;
    }
}

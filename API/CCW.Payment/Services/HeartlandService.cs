using Azure.Security.KeyVault.Secrets;
using GlobalPayments.Api;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.PaymentMethods;

namespace CCW.Payment.Services;

public class HeartlandService : IHeartlandService
{
    private readonly string _apiKey;
    private readonly SecretClient _client;

    public HeartlandService(SecretClient client)
    {
        _client = client;
        _apiKey = _client.GetSecret("HeartlandSandboxAPISecretKey").Value.Value;

        ServicesContainer.ConfigureService(new PorticoConfig
        {
            SecretApiKey = _apiKey,
            DeveloperId = "000000",
            VersionNumber = "0000",
            ServiceUrl = "https://cert.api2.heartlandportico.com"
        });
    }

    public Transaction ChargeCard(string token, decimal amount)
    {
        var card = new CreditCardData
        {
            Token = token
        };

        var response = card.Charge(amount)
            .WithCurrency("USD")
            .Execute();

        return response;
    }

    public Transaction RefundPayment(string transactionId, decimal amount)
    {
        var response = Transaction.FromId(transactionId).Refund(amount).WithCurrency("USD").Execute();

        return response;
    }
}

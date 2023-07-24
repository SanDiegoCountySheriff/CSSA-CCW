using CCW.Common.Models;

namespace CCW.Payment.Clients;

public class ApplicationServiceClient : IApplicationServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string _updateApplicationPaymentHistoryUrl;
    private readonly string _updateUserApplicationPaymentHistoryUrl;

    public ApplicationServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _updateApplicationPaymentHistoryUrl = configuration.GetSection("ApplicationServiceClient").GetSection("UpdateApplicationPaymentHistoryUrl").Value;
        _updateUserApplicationPaymentHistoryUrl = configuration.GetSection("ApplicationServiceClient").GetSection("UpdateUserApplicationPaymentHistoryUrl").Value;
    }

    public async Task<HttpResponseMessage> UpdateApplicationPaymentHistoryAsync(PaymentHistory paymentHistory, string applicationId, string userId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, _updateApplicationPaymentHistoryUrl + $"?applicationId={applicationId}&userId={userId}")
        {
            Content = JsonContent.Create(paymentHistory)
        };

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> UpdateUserApplicationPaymentHistoryAsync(PaymentHistory paymentHistory, string applicationId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, _updateUserApplicationPaymentHistoryUrl + $"?applicationId={applicationId}")
        {
            Content = JsonContent.Create(paymentHistory)
        };

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }
}

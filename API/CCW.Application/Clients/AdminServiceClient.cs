using System.Net;
using System.Text.Json;
using CCW.Application.Models;

namespace CCW.Application.Clients;

public class AdminServiceClient : IAdminServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string uri;

    public AdminServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        uri = configuration.GetSection("AdminServiceClient").GetSection("BaseUrl").Value;
    }

    public async Task<AgencyProfileSettingsModel> GetAgencyProfileSettingsAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return await JsonSerializer.DeserializeAsync<AgencyProfileSettingsModel>(
            await result.Content.ReadAsStreamAsync(cancellationToken),
            cancellationToken: cancellationToken) ?? throw new HttpRequestException(
            "GetAgencyProfileSettings returned null or response body could not be deserialized.", null, HttpStatusCode.InternalServerError);

    }
}
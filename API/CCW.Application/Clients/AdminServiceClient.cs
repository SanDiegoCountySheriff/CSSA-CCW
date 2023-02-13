using System.Net;
using System.Text.Json;
using CCW.Application.Models;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

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
        AgencyProfileSettingsModel response = new AgencyProfileSettingsModel();

        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        if (result.IsSuccessStatusCode)
        {
            var jstring = await result.Content.ReadAsStringAsync();
            response = JsonConvert.DeserializeObject<AgencyProfileSettingsModel>(jstring);

            return response;
        }

        throw new HttpRequestException(
            "GetAgencyProfileSettings request could not be completed.", null, HttpStatusCode.NotFound);

    }
}
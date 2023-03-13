using CCW.Application.Entities;
using CCW.Application.Models;
using Newtonsoft.Json;
using System.Net;

namespace CCW.Application.Clients
{
    public class UserProfileServiceClient : IUserProfileServiceClient
    {

        private readonly HttpClient _httpClient;
        private readonly string getAdminUserUri;

        public UserProfileServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            getAdminUserUri = configuration.GetSection("UserProfileServiceClient").GetSection("GetAdminUser").Value;
        }

        public async Task<AdminUser> GetAdminUserProfileAsync(CancellationToken cancellationToken)
        {
            AdminUser response = new AdminUser();

            var request = new HttpRequestMessage(HttpMethod.Get, getAdminUserUri);

            var result = await _httpClient.SendAsync(request, cancellationToken);
            result.EnsureSuccessStatusCode();

            if (result.IsSuccessStatusCode)
            {
                var jstring = await result.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<AdminUser>(jstring);

                return response;
            }

            throw new HttpRequestException(
                "GetAdminUser request could not be completed.", null, HttpStatusCode.NotFound);

        }
    }
}

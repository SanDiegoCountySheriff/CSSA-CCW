using Newtonsoft.Json;
namespace CCW.UserProfile.Models;

public class UserProfileResponseModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
}


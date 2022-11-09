using Newtonsoft.Json;

namespace CCW.UserProfile.Entities;

public class User
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
}
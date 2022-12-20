using CCW.UserProfile.Entities;
using Newtonsoft.Json;
namespace CCW.UserProfile.Models;

public class UserProfileResponseModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("userEmail")]
    public string UserEmail { get; set; }
    [JsonProperty("previousEmails")]
    public Email[] PreviousEmails { get; set; }
}


using Newtonsoft.Json;

namespace CCW.UserProfile.Entities;

public class User
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("email")]
    public string UserEmail { get; set; }
    [JsonProperty("previousEmails")]
    public Email[] PreviousEmails { get; set; }
    [JsonProperty("userCreateDateTime")]
    public DateTime UserCreateDateTimeUtc { get; set; }
    [JsonProperty("profileUpdateDateTime")]
    public DateTime ProfileUpdateDateTimeUtc { get; set; }
}
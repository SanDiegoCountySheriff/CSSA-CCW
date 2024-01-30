using CCW.Common.Models;
using Newtonsoft.Json;

namespace CCW.Common.ResponseModels;

public class UserProfileResponseModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("userEmail")]
    public string UserEmail { get; set; }
    [JsonProperty("previousEmails")]
    public Email[] PreviousEmails { get; set; }
}


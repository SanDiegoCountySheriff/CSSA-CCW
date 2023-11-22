using Newtonsoft.Json;

namespace CCW.Common.RequestModels;

public class UserProfileRequestModel
{
    [JsonProperty("emailAddress")]
    public string EmailAddress { get; set; }
}

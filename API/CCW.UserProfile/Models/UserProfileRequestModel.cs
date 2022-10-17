using Newtonsoft.Json;

namespace CCW.UserProfile.Models
{
    public class UserProfileRequestModel
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
    }
}

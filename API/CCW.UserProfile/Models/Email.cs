using Newtonsoft.Json;

namespace CCW.UserProfile.Models
{
    public class Email
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
    }
}

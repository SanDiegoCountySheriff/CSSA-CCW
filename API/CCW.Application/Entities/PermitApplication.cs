using Newtonsoft.Json;

namespace CCW.Application.Entities
{
    public class PermitApplication
    {
        public Application Application { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }
        public string Email
        {
            get
            {
                return Application.UserEmail ?? "";
            }
        }
        public bool IsComplete
        {
            get
            {
                return Application.IsComplete;
            }
        }
    }
}

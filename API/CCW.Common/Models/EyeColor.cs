using Newtonsoft.Json;

namespace CCW.Admin.Entities
{
    public class EyeColor
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
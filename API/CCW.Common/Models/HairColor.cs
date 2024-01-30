using Newtonsoft.Json;

namespace CCW.Admin.Entities
{
    public class HairColor
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

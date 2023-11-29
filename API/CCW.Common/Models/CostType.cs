using Newtonsoft.Json;

namespace CCW.Common.Models
{
    public class CostType
    {
        [JsonProperty("standard")]
        public decimal Standard { get; set; }
        [JsonProperty("judicial")]
        public decimal Judicial { get; set; }
        [JsonProperty("reserve")]
        public decimal Reserve { get; set; }
    }
}

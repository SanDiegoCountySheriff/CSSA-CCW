using Newtonsoft.Json;

namespace CCW.Common.Models;

public class CostType
{
    [JsonProperty("standard")]
    public float Standard { get; set; }
    [JsonProperty("judicial")]
    public float Judicial { get; set; }
    [JsonProperty("reserve")]
    public float Reserve { get; set; }
    [JsonProperty("employment")]
    public float Employment { get; set; }
}

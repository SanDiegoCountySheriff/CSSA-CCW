using Newtonsoft.Json;

namespace CCW.Admin.Entities;

public class CostType
{
    [JsonProperty("standard")]
    public int Standard { get; set; }
    [JsonProperty("judicial")]
    public int Judicial { get; set; }
    [JsonProperty("reserve")]
    public int Reserve { get; set; }
}
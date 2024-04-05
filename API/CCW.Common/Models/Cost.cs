using Newtonsoft.Json;

namespace CCW.Common.Models;

public class Cost
{
    [JsonProperty("new")]
    public CostType New { get; set; }
    [JsonProperty("renew")]
    public CostType Renew { get; set; }
    [JsonProperty("issuance")]
    public float Issuance { get; set; }
    [JsonProperty("modify")]
    public float Modify { get; set; }
    [JsonProperty("creditFee")]
    public float CreditFee { get; set; }
}

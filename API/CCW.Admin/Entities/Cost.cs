using Newtonsoft.Json;

namespace CCW.Admin.Entities;

public class Cost
{
    [JsonProperty("new")]
    public CostType New { get; set; }
    [JsonProperty("renew")]
    public CostType Renew { get; set; }
    [JsonProperty("issuance")]
    public int Issuance { get; set; }
    [JsonProperty("modify")]
    public int Modify { get; set; }
    [JsonProperty("creditFee")]
    public int CreditFee { get; set; }
    [JsonProperty("convenienceFee")]
    public int ConvenienceFee { get; set; }
}
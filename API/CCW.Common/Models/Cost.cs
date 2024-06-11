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
    [JsonProperty("standardLivescanFee")]
    public float? StandardLivescanFee { get; set; }
    [JsonProperty("judicialLivescanFee")]
    public float? JudicialLivescanFee { get; set; }
    [JsonProperty("reserveLivescanFee")]
    public float? ReserveLivescanFee { get; set; }
    [JsonProperty("employmentLivescanFee")]
    public float? EmploymentLivescanFee { get; set; }
}

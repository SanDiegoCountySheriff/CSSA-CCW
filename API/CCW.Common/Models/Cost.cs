using Newtonsoft.Json;

namespace CCW.Common.Models
{
    public class Cost
    {
        [JsonProperty("new")]
        public CostType New { get; set; }
        [JsonProperty("renew")]
        public CostType Renew { get; set; }
        [JsonProperty("issuance")]
        public decimal Issuance { get; set; }
        [JsonProperty("modify")]
        public decimal Modify { get; set; }
        [JsonProperty("creditFee")]
        public decimal CreditFee { get; set; }
        [JsonProperty("convenienceFee")]
        public decimal ConvenienceFee { get; set; }
    }
}

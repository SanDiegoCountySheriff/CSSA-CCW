using Newtonsoft.Json;

namespace CCW.Common.Models;

public class RefundRequest
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("orderId")]
    public string OrderId { get; set; }
    [JsonProperty("transactionId")]
    public string TransactionId { get; set; }
    [JsonProperty("applicationId")]
    public string ApplicationId { get; set; }
    [JsonProperty("refundAmount")]
    public decimal RefundAmount { get; set; }
    [JsonProperty("reason")]
    public string Reason { get; set; }
}

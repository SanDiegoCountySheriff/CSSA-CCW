using Newtonsoft.Json;

namespace CCW.Application.Entities;

public class PaymentHistory
{
    [JsonProperty("amount")]
    public string Amount { get; set; }
    [JsonProperty("paymentDateTimeUtc")]
    public DateTime PaymentDateTimeUtc { get; set; }
    [JsonProperty("recordedBy")]
    public string RecordedBy { get; set; }
    [JsonProperty("comments")]
    public string Comments { get; set; }
}

public class PaymentHistoryResponse
{
    public PaymentHistory[] PaymentHistory { get; set; }
}
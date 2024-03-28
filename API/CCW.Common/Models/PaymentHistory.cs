using CCW.Common.Enums;
using Newtonsoft.Json;

namespace CCW.Common.Models;

public class PaymentHistory
{
    [JsonProperty("paymentDateTimeUtc")]
    public DateTimeOffset PaymentDateTimeUtc { get; set; }
    [JsonProperty("paymentType")]
    public PaymentType PaymentType { get; set; }
    [JsonProperty("vendorInfo")]
    public string VendorInfo { get; set; }
    [JsonProperty("amount")]
    public decimal Amount { get; set; }
    [JsonProperty("refundAmount")]
    public decimal RefundAmount { get; set; }
    [JsonProperty("recordedBy")]
    public string RecordedBy { get; set; }
    [JsonProperty("transactionId")]
    public string TransactionId { get; set; }
    [JsonProperty("successful")]
    public bool Successful { get; set; }
    [JsonProperty("paymentStatus")]
    public PaymentStatus PaymentStatus { get; set; }
}

public class PaymentHistoryResponse
{
    public PaymentHistory[] PaymentHistory { get; set; }
}
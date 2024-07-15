using CCW.Common.Enums;
using Newtonsoft.Json;

namespace CCW.Application.ResponseModels;

public class PaymentHistoryResponseModel
{
    [JsonProperty("paymentDateTimeUtc")]
    public DateTimeOffset PaymentDateTimeUtc { get; set; }
    [JsonProperty("paymentType")]
    public string PaymentType { get; set; }
    [JsonProperty("vendorInfo")]
    public string VendorInfo { get; set; }
    [JsonProperty("amount")]
    public string Amount { get; set; }
    [JsonProperty("recordedBy")]
    public string RecordedBy { get; set; }
    [JsonProperty("transactionId")]
    public string TransactionId { get; set; }
    [JsonProperty("paymentStatus")]
    public PaymentStatus PaymentStatus { get; set; }
    [JsonProperty("modificationNumber")]
    public int? ModificationNumber { get; set; }
    [JsonProperty("verified")]
    public bool? Verified { get; set; }
}

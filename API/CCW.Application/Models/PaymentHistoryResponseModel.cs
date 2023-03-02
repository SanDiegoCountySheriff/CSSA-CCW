using Newtonsoft.Json;

namespace CCW.Application.Models;

public class PaymentHistoryResponseModel
{
    [JsonProperty("paymentDateTimeUtc")]
    public DateTime PaymentDateTimeUtc { get; set; }

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
}

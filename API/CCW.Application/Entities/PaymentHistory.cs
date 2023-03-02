using Newtonsoft.Json;

namespace CCW.Application.Entities;

public class PaymentHistory
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

public class PaymentHistoryResponse
{
    public PaymentHistory[] PaymentHistory { get; set; }
}
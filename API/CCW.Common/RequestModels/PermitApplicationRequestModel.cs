using CCW.Common.Models;
using Newtonsoft.Json;

namespace CCW.Application.Models;

public class PermitApplicationRequestModel
{
    [JsonProperty("application")]
    public Common.Models.Application Application { get; set; }
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
    [JsonProperty("paymentHistory")]
    public PaymentHistory[] PaymentHistory { get; set; }
    [JsonProperty("history")]
    public History[] History { get; set; }
    public DateTimeOffset? HistoricalDate { get; set; }
    public bool? IsMatchUpdated { get; set; }
}
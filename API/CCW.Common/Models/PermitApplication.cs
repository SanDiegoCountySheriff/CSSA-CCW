using Newtonsoft.Json;

namespace CCW.Common.Models;

public class PermitApplication
{
    public Application Application { get; set; }
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
    public List<PaymentHistory> PaymentHistory { get; set; }
    public History[] History { get; set; }
    public DateTimeOffset? HistoricalDate { get; set; }
    public bool? IsMatchUpdated { get; set; }
}
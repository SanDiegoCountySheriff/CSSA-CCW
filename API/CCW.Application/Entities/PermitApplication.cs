using Newtonsoft.Json;

namespace CCW.Application.Entities;

public class PermitApplication
{
    [JsonProperty("application")]
    public Application Application { get; set; }
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
    [JsonProperty("paymentHistory")]
    public PaymentHistory[] PaymentHistory { get; set; }
    [JsonProperty("history")]
    public History[] History { get; set; }
}
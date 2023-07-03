using Newtonsoft.Json;

namespace CCW.Application.Entities;

public class PermitApplication
{
    public Application Application { get; set; }
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
    public PaymentHistory[] PaymentHistory { get; set; }
    public History[] History { get; set; }
}
using CCW.Common.Models;
using Newtonsoft.Json;

namespace CCW.Application.ResponseModels;

public class PermitApplicationResponseModel
{
    public Common.Models.Application Application { get; set; }
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
    public History[] History { get; set; }
    public PaymentHistory[] PaymentHistory { get; set; }
    public DateTimeOffset? HistoricalDate { get; set; }
    public bool? IsMatchUpdated { get; set; }
}
using CCW.Common.Models;
using Newtonsoft.Json;

namespace CCW.Application.Models;

public class UserPermitApplicationRequestModel
{
    public UserApplication Application { get; set; }
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
    [JsonProperty("paymentHistory")]
    public PaymentHistory[] PaymentHistory { get; set; }
    public bool? IsMatchUpdated { get; set; }
    public string ETag { get; set; }
}
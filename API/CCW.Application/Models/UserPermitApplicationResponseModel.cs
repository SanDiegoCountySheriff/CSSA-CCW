using CCW.Application.Entities;
using CCW.Common.Models;
using Newtonsoft.Json;

namespace CCW.Application.Models;

public class UserPermitApplicationResponseModel
{
    [JsonProperty("application")]
    public UserApplication Application { get; set; }
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
    [JsonProperty("paymentHistory")]
    public List<PaymentHistory> PaymentHistory { get; set; }
}
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace CCW.Admin.Models;

public class AgencyProfileSettings
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("agencyName")]
    public string AgencyName { get; set; }
    [JsonProperty("agencySheriffName")]
    public string AgencySheriffName { get; set; }
    [JsonProperty("chiefOfPoliceName")]
    public string ChiefOfPoliceName { get; set; }
    [JsonProperty("primaryThemeColor")]
    public string PrimaryThemeColor { get; set; }
    [JsonProperty("secondaryThemeColor")]
    public string SecondaryThemeColor { get; set; }
    [JsonProperty("agencyLogo")]
    public string? AgencyLogo { get; set; }
    [JsonProperty("standardCost")]
    public int StandardCost { get; set; }
    [JsonProperty("initialCost")]
    public int InitialCost { get; set; }
    [JsonProperty("reserveCost")]
    public int ReserveCost { get; set; }
    [JsonProperty("creditFee")]
    public int CreditFee { get; set; }
    [JsonProperty("convenienceFee")]
    public int ConvenienceFee { get; set; }
    [JsonProperty("paymentURL")]
    public string PaymentURL { get; set; }
    [JsonProperty("refreshTokenTime")]
    public int RefreshTokenTime { get; set; }
}
using Newtonsoft.Json;

namespace CCW.Admin.Entities;

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
    [JsonProperty("agencyAddress")]
    public string AgencyAddress { get; set; }
    [JsonProperty("agencyTelephone")]
    public string AgencyTelephone { get; set; }
    [JsonProperty("agencyFax")]
    public string AgencyFax { get; set; }
    [JsonProperty("agencyEmail")]
    public string AgencyEmail { get; set; }
    [JsonProperty("primaryThemeColor")]
    public string PrimaryThemeColor { get; set; }
    [JsonProperty("secondaryThemeColor")]
    public string SecondaryThemeColor { get; set; }
    public Cost Cost { get; set; }
    [JsonProperty("paymentURL")]
    public string PaymentURL { get; set; }
    [JsonProperty("refreshTokenTime")]
    public int RefreshTokenTime { get; set; }
}
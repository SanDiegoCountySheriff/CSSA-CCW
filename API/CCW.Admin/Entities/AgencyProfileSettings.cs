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
    [JsonProperty("ori")]
    public string ORI { get; set; }
    [JsonProperty("courthouse")]
    public string Courthouse { get; set; }
    [JsonProperty("localAgencyNumber")]
    public string LocalAgencyNumber { get; set; }
    [JsonProperty("agencyBillingNumber")]
    public string AgencyBillingNumber { get; set; }
    [JsonProperty("contactName")]
    public string ContactName { get; set; }
    [JsonProperty("contactNumber")]
    public string ContactNumber { get; set; }
    [JsonProperty("mailCode")]
    public string MailCode { get; set; }
    [JsonProperty("agencyStreetAddress")]
    public string AgencyStreetAddress { get; set; }
    [JsonProperty("agencyAptBuildingNumber")]
    public string AgencyAptBuildingNumber { get; set; }
    [JsonProperty("agencyCity")]
    public string AgencyCity { get; set; }
    [JsonProperty("agencyState")]
    public string AgencyState { get; set; }
    [JsonProperty("agencyZip")]
    public string AgencyZip { get; set; }
    [JsonProperty("agencyCounty")]
    public string AgencyCounty { get; set; }
    [JsonProperty("agencyShippingStreetAddress")]
    public string AgencyShippingStreetAddress { get; set; }
    [JsonProperty("agencyShippingAptBuildingNumber")]
    public string AgencyShippingAptBuildingNumber { get; set; }
    [JsonProperty("agencyShippingCity")]
    public string AgencyShippingCity { get; set; }
    [JsonProperty("agencyShippingState")]
    public string AgencyShippingState { get; set; }
    [JsonProperty("agencyShippingZip")]
    public string AgencyShippingZip { get; set; }
    [JsonProperty("agencyShippingCounty")]
    public string AgencyShippingCounty { get; set; }
}
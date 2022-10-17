using Newtonsoft.Json;

namespace CCW.Admin.Models;
public class AgencyProfileRequestModel
{
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
    [JsonProperty("agencyLogoDataURL")]
    public string AgencyLogoDataURL { get; set; }
}

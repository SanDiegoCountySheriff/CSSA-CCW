using CCW.Common.Models;
using Newtonsoft.Json;

namespace CCW.Common.RequestModels;

public class AdminUserProfileRequestModel
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("badgeNumber")]
    public string BadgeNumber { get; set; }
    [JsonProperty("jobTitle")]
    public string JobTitle { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[] UploadedDocuments { get; set; }
}

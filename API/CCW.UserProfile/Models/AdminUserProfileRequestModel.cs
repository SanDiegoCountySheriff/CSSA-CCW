using CCW.UserProfile.Entities;
using Newtonsoft.Json;

namespace CCW.UserProfile.Models;

public class AdminUserProfileRequestModel
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("badgeNumber")]
    public string BadgeNumber { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[]? UploadedDocuments { get; set; }
}

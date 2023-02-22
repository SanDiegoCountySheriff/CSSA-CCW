using CCW.UserProfile.Entities;
using Newtonsoft.Json;

namespace CCW.UserProfile.Models;

public class AdminUserProfileResponseModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("badgeNumber")]
    public string BadgeNumber { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[] UploadedDocuments { get; set; }
}

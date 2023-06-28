using Newtonsoft.Json;

namespace CCW.UserProfile.Entities;

public class AdminUser
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("badgeNumber")]
    public string BadgeNumber { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[]? UploadedDocuments { get; set; }
}

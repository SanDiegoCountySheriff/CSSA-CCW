using Newtonsoft.Json;

namespace CCW.Application.Entities;

public class AdminUser
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("badgeNumber")]
    public string BadgeNumber { get; set; }
    [JsonProperty("jobTitle")]
    public string JobTitle { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[]? UploadedDocuments { get; set; }
}

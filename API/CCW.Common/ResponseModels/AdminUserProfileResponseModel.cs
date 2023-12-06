using CCW.Common.Models;
using Newtonsoft.Json;

namespace CCW.Common.ResponseModels;

public class AdminUserProfileResponseModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("badgeNumber")]
    public string BadgeNumber { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("jobTitle")]
    public string JobTitle { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[] UploadedDocuments { get; set; }
}

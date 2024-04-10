using Newtonsoft.Json;

namespace CCW.Common.Models;

public class UserProfile
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("address")]
    public Address Address { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("dateOfBirth")]
    public string DateOfBirth { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[] UploadedDocuments { get; set; }
}

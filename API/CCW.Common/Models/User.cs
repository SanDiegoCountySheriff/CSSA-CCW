using Newtonsoft.Json;

namespace CCW.Common.Models;

public class User
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("address")]
    public Address Address { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("dateOfBirth")]
    public string DateOfBirth { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[] UploadedDocuments { get; set; }
}

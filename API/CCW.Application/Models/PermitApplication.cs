using Newtonsoft.Json;

namespace CCW.Application.Models;

public class PermitApplication
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
    [JsonProperty("firstName")]
    public string FirstName { get; set; }
    [JsonProperty("lastName")]
    public string LastName { get; set; }
    [JsonProperty("address")]
    public string Address { get; set; }
    [JsonProperty("city")]
    public string City { get; set; }
    [JsonProperty("zipCode")]
    public string ZipCode { get; set; }
    [JsonProperty("state")]
    public string State { get; set; }
    [JsonProperty("permitType")]
    public string PermitType { get; set; }
}

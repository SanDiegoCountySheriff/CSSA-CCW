using CCW.Common.Models;
using Newtonsoft.Json;

namespace CCW.Common.RequestModels;

public class UserProfileRequestModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("address")]
    public Address Address { get; set; }
    [JsonProperty("firstName")]
    public string FirstName { get; set; }
    [JsonProperty("lastName")]
    public string LastName { get; set; }
    [JsonProperty("dateOfBirth")]
    public string DateOfBirth { get; set; }
    [JsonProperty("driversLicenseNumber")]
    public string DriversLicenseNumber { get; set; }
    [JsonProperty("permitNumber")]
    public string PermitNumber { get; set; }
    [JsonProperty("appointmentDate")]
    public DateTime AppointmentDate { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[] UploadedDocuments { get; set; }
}

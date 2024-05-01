using Newtonsoft.Json;

namespace CCW.Common.Models;

public class User
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("firstName")]
    public string FirstName { get; set; }
    [JsonProperty("lastName")]
    public string LastName { get; set; }
    [JsonProperty("middleName")]
    public string MiddleName { get; set; }
    [JsonProperty("dateOfBirth")]
    public string DateOfBirth { get; set; }
    [JsonProperty("driversLicenseNumber")]
    public string DriversLicenseNumber { get; set; }
    [JsonProperty("permitNumber")]
    public string PermitNumber { get; set; }
    [JsonProperty("appointmentDate")]
    public DateTimeOffset? AppointmentDate { get; set; }
    [JsonProperty("appointmentTime")]
    public string AppointmentTime { get; set; }
    [JsonProperty("uploadedDocuments")]
    public UploadedDocument[] UploadedDocuments { get; set; }
    [JsonProperty("isPendingReview")]
    public Boolean IsPendingReview { get; set; }

}

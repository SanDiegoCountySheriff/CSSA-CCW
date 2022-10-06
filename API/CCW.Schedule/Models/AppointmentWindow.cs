using Newtonsoft.Json;

namespace CCW.Schedule.Models;

public class AppointmentWindow
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("start")]
    public DateTime Start { get; set; }
    [JsonProperty("end")]
    public DateTime End { get; set; }
    [JsonProperty("applicantName")]
    public string? ApplicantName { set; get; }
    [JsonProperty("applicantId")]
    public string? ApplicantId { set; get; }
    [JsonProperty("isManuallyCreated")]
    public bool IsManuallyCreated { get; set; }
}
using Newtonsoft.Json;

namespace CCW.Schedule.Models;

public class AppointmentWindowCreateRequestModel
{
    [JsonProperty("start")]
    public DateTime Start { get; set; }

    [JsonProperty("end")]
    public DateTime End { get; set; }

    [JsonProperty("applicationId")]
    public string? ApplicationId { set; get; }

    [JsonProperty("status")]
    public string? Status { set; get; }

    [JsonProperty("name")]
    public string? Name { set; get; }

    [JsonProperty("permit")]
    public string? Permit { set; get; }

    [JsonProperty("payment")]
    public string? Payment { set; get; }

    [JsonProperty("isManuallyCreated")]
    public bool IsManuallyCreated { get; set; }
}
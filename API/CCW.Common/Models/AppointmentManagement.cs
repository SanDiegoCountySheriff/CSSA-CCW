using Newtonsoft.Json;

namespace CCW.Common.Models;

public class AppointmentManagement
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("daysOfTheWeek")]
    public List<string> DaysOfTheWeek { get; set; }
    [JsonProperty("firstAppointmentStartTime")]
    public string FirstAppointmentStartTime { get; set; }
    [JsonProperty("lastAppointmentStarttime")]
    public string LastAppointmentStartTime { get; set; }
    [JsonProperty("numberOfSlotsPerAppointment")]
    public int NumberOfSlotsPerAppointment { get; set; }
    [JsonProperty("appointmentLength")]
    public int AppointmentLength { get; set; }
    [JsonProperty("numberOfWeeksToCreate")]
    public int NumberOfWeeksToCreate { get; set; }
    [JsonProperty("breakLength")]
    public int? BreakLength { get; set; }
    [JsonProperty("breakStartTime")]
    public string? BreakStartTime { get; set; }
    [JsonProperty("startDate")]
    public DateTimeOffset StartDate { get; set; }
}

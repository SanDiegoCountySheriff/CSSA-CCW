using Newtonsoft.Json;

namespace CCW.Schedule.Entities;

public class AppointmentManagement
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("daysOfTheWeek")]
    public List<string> DaysOfTheWeek { get; set; }
    [JsonProperty("firstAppointmentStartTime")]
    public TimeSpan FirstAppointmentStartTime { get; set; }
    [JsonProperty("lastAppointmentStarttime")]
    public TimeSpan LastAppointmentStartTime { get; set; }
    [JsonProperty("numberOfSlotsPerAppointment")]
    public int NumberOfSlotsPerAppointment { get; set; }
    [JsonProperty("appointmentLength")]
    public int AppointmentLength { get; set; }
    [JsonProperty("numberOfWeeksToCreate")]
    public int NumberOfWeeksToCreate { get; set; }
    [JsonProperty("breakLength")]
    public int? BreakLength { get; set; }
    [JsonProperty("breakStartTime")]
    public TimeSpan? BreakStartTime { get; set; }
    [JsonProperty("startDate")]
    public DateTime StartDate { get; set; }
}

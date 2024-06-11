using Newtonsoft.Json;

namespace CCW.Common.RequestModels;

public class AppointmentManagementRequestModel
{
    [JsonProperty("daysOfTheWeek")]
    public List<string> DaysOfTheWeek { get; set; }
    [JsonProperty("firstAppointmentStartTime")]
    public DateTimeOffset FirstAppointmentStartTime { get; set; }
    [JsonProperty("lastAppointmentStarttime")]
    public DateTimeOffset LastAppointmentStartTime { get; set; }
    [JsonProperty("numberOfSlotsPerAppointment")]
    public int NumberOfSlotsPerAppointment { get; set; }
    [JsonProperty("appointmentLength")]
    public int AppointmentLength { get; set; }
    [JsonProperty("numberOfWeeksToCreate")]
    public int NumberOfWeeksToCreate { get; set; }
    [JsonProperty("breakLength")]
    public int? BreakLength { get; set; }
    [JsonProperty("breakStartTime")]
    public DateTimeOffset? BreakStartTime { get; set; }
    [JsonProperty("startDate")]
    public DateTimeOffset StartDate { get; set; }
}


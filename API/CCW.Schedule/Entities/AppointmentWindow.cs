using CCW.Common.Enums;
using Newtonsoft.Json;

namespace CCW.Schedule.Entities;

public class AppointmentWindow
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("start")]
    public DateTimeOffset Start { get; set; }
    
    [JsonProperty("end")]
    public DateTimeOffset End { get; set; }

    [JsonProperty("appointmentCreatedDate")]
    public DateTimeOffset? AppointmentCreatedDate { get; set; }
    
    [JsonProperty("applicationId")]
    public string ApplicationId { set; get; }
    
    [JsonProperty("status")]
    public AppointmentStatus Status { set; get; }
    
    [JsonProperty("name")]
    public string Name { set; get; }
    
    [JsonProperty("permit")]
    public string Permit { set; get; }
    
    [JsonProperty("payment")]
    public string Payment { set; get; }
    
    [JsonProperty("isManuallyCreated")]
    public bool IsManuallyCreated { get; set; }

    [JsonProperty("userId")]
    public string UserId { get; set; }

    [JsonIgnore]
    public string _etag { get; set; }
}
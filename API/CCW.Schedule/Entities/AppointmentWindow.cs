using Newtonsoft.Json;

namespace CCW.Schedule.Entities;

public class AppointmentWindow
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("start")]
    public string Start { get; set; }
    
    [JsonProperty("end")]
    public string End { get; set; }
    
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

    [JsonIgnore]
    public string _etag { get; set; }
}
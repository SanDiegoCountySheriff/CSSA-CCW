
using Newtonsoft.Json;

namespace CCW.CosmosDb.Seed.Model;

public class AppointmentModel
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
    //[JsonPropertyName("agent")]
    //public AgentModel Agent { get; set; }
}

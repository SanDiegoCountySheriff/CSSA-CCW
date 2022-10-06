using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CCW.Schedule.Models;

public class AppointmentWindow
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    [JsonIgnore()]
    public Agent Agent { get; set; }

    [JsonPropertyName("text")]
    public string? ApplicantName { set; get; }

    [JsonPropertyName("client")]
    public string? ApplicantId { set; get; }

    public string Status { get; set; } = "free";

    [NotMapped]
    public int Resource { get { return Agent.Id; } }

    [NotMapped]
    public string AgentName { get { return Agent.Name; } }
}
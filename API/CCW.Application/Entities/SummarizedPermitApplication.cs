using CCW.Application.Enum;
using Newtonsoft.Json;

namespace CCW.Application.Entities;

public class SummarizedPermitApplication
{
    public string UserEmail { get; set; }
    public Address? CurrentAddress { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public ApplicationStatus? Status { get; set; }
    public AppointmentStatus? AppointmentStatus { get; set; }
    public string? OrderId { get; set; }
    [JsonProperty("id")]
    public string id { get; set; }
}
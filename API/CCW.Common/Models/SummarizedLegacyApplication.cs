using CCW.Common.Enums;
using Newtonsoft.Json;

namespace CCW.Common.Models;

public class SummarizedLegacyApplication
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string IdNumber { get; set; }
    public string BirthDate { get; set; }
    public string PermitNumber { get; set; }
    public string Email { get; set; }
    public ApplicationStatus Status { get; set; }
    public ApplicationType ApplicationType { get; set; }
    public DateTimeOffset? AppointmentDateTime { get; set; }
}

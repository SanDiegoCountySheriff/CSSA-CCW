using CCW.Application.Entities;
using CCW.Application.Enum;

namespace CCW.Application.Models;

public class SummarizedPermitApplicationResponseModel
{
    public string ApplicationID { get; set; }
    public string? OrderID { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Email { get; set; }
    public Address? Address { get; set; }
    public int? Status { get; set; }
    public bool? AppointmentStatus { get; set; }
    public DateTime? AppointmentDateTime { get; set; }
    public DOB? DOB { get; set; }
    public bool? IsComplete { get; set; }
    public string? ApplicationType { get; set; }
}
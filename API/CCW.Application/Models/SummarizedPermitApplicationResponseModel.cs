using CCW.Application.Entities;
using CCW.Application.Enum;

namespace CCW.Application.Models;

public class SummarizedPermitApplicationResponseModel
{
    public string ApplicationID { get; set; }
    public string OrderID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public ApplicationStatus Status { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
}
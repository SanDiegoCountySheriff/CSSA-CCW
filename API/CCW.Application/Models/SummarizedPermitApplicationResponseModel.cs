using CCW.Application.Entities;
using CCW.Application.Enum;
using CCW.Common.Models;

namespace CCW.Application.Models;

public class SummarizedPermitApplicationResponseModel
{
    public string id { get; set; }
    public string OrderId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string UserEmail { get; set; }
    public Address CurrentAddress { get; set; }
    public ApplicationStatus Status { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public string ApplicationType { get; set; }
    public bool IsComplete { get; set; }
    public DOB DOB { get; set; }
    public DateTime? AppointmentDateTime { get; set; }
    public string UserId { get; set; }
    public string AssignedTo { get; set; }
    public bool FlaggedForCustomerReview { get; set; }
    public bool FlaggedForLicensingReview { get; set; }

}
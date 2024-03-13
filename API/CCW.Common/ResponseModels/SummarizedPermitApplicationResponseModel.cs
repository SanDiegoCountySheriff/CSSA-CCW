using CCW.Common.Enums;

namespace CCW.Application.Models;

public class SummarizedPermitApplicationResponseModel
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public ApplicationStatus Status { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public bool Paid { get; set; }
    public string ApplicationType { get; set; }
    public bool IsComplete { get; set; }
    public DateTime? AppointmentDateTime { get; set; }
    public string AssignedTo { get; set; }
    public bool FlaggedForCustomerReview { get; set; }
    public bool FlaggedForLicensingReview { get; set; }
}

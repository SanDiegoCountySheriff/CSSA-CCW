using CCW.Common.Enums;

namespace CCW.Common.ResponseModels;

public class ApplicationSummaryCountResponseModel
{
    public int StandardType { get; set; }
    public int ReserveType { get; set; }
    public int JudicialType { get; set; }
    public int EmploymentType { get; set; }
    public int SuspendedStatus { get; set; }
    public int RevokedStatus { get; set; }
    public int DeniedStatus { get; set; }
    public int ActiveStandardStatus { get; set; }
    public int ActiveReserveStatus { get; set; }
    public int ActiveJudicialStatus { get; set; }
    public int ActiveEmploymentStatus { get; set; }
    public int SubmittedStatus { get; set; }
}

public class AssignedApplicationSummary
{
    public string OrderId { get; set; }
    public string Name { get; set; }
    public ApplicationStatus Status { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
}

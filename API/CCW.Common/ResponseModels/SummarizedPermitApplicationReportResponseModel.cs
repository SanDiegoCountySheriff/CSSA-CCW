using CCW.Common.Enums;
using CCW.Common.Models;

namespace CCW.Application.ResponseModels;

public class SummarizedPermitApplicationReportResponseModel
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string Suffix { get; set; }
    public ApplicationStatus Status { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public bool Paid { get; set; }
    public ApplicationType ApplicationType { get; set; }
    public DateTime? AppointmentDateTime { get; set; }
    public List<Alias> Aliases { get; set; }
    public string BirthDate { get; set; }
}

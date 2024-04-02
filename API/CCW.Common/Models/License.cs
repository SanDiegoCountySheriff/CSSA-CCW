namespace CCW.Common.Models;

public class License
{
    public string PermitNumber { get; set; }
    public string IssuingCounty { get; set; }
    public DateTimeOffset? ExpirationDate { get; set; }
    public DateTimeOffset? IssueDate { get; set; }
}

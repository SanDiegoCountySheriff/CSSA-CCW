namespace CCW.Common.Models;

public class QualifyingQuestionTwelve
{
    public bool? Selected { get; set; }
    public List<TrafficViolation> TrafficViolations { get; set; } = new List<TrafficViolation>();
    public List<TrafficViolation> TemporaryTrafficViolations { get; set; } = new List<TrafficViolation>();
    public string TrafficViolationsExplanation { get; set; }
    public bool? UpdateInformation { get; set; }
}

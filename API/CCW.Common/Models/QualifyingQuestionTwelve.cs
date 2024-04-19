namespace CCW.Common.Models;

public class QualifyingQuestionTwelve
{
    public bool? Selected { get; set; }
    public List<TrafficViolation> TrafficViolations { get; set; }
    public List<TrafficViolation> TemporaryTrafficViolations { get; set; }
    public string TrafficViolationsExplanation { get; set; }
    public bool? UpdateInformation { get; set; }
}

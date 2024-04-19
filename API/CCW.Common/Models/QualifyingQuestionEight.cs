namespace CCW.Common.Models;

public class QualifyingQuestionEight
{
    public bool? Selected { get; set; }
    public List<TrafficViolation> TrafficViolations { get; set; }
    public List<TrafficViolation> TemporaryTrafficViolations { get; set; }
    public string TrafficViolationsExplanation { get; set; }
    public bool? UpdateInformation { get; set; }
}

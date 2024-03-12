namespace CCW.Common.Models;

public class QualifyingQuestionEight
{
    public bool? Selected { get; set; }
    public List<TrafficViolation> TrafficViolations { get; set; }
    public List<TrafficViolation> TemporaryTrafficViolations { get; set; }
    public bool? HasChanges { get; set; }
}

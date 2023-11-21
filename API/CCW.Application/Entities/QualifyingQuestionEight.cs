namespace CCW.Application.Entities;

public class QualifyingQuestionEight
{
    public bool? Selected { get; set; }
    public List<TrafficViolation> TrafficViolations { get; set; }
    public List<TrafficViolation> TemporaryTrafficViolations { get; set; }
}

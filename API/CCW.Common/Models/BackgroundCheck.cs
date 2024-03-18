using System;

namespace CCW.Common.Models;

public class BackgroundCheck
{
    public BackgroundCheckItem ProofOfID { get; set; }
    public BackgroundCheckItem ProofOfResidency { get; set; }
    public BackgroundCheckItem NCICWantsWarrants { get; set; }
    public BackgroundCheckItem Locals { get; set; }
    public BackgroundCheckItem Probations { get; set; }
    public BackgroundCheckItem DMVRecord { get; set; }
    public BackgroundCheckItem AKAsChecked { get; set; }
    public BackgroundCheckItem CrimeTracer { get; set; }
    public BackgroundCheckItem TrafficCourtPortal { get; set; }
    public BackgroundCheckItem DOJApprovalLetter { get; set; }
    public BackgroundCheckItem CIINumber { get; set; }
    public BackgroundCheckItem DOJ { get; set; }
    public BackgroundCheckItem FBI { get; set; }
    public BackgroundCheckItem Livescan { get; set; }
    public BackgroundCheckItem SR14 { get; set; }
    public BackgroundCheckItem Firearms { get; set; }
    public BackgroundCheckItem SidLettersReceived { get; set; }
    public BackgroundCheckItem SafetyCertificate { get; set; }
    public BackgroundCheckItem Restrictions { get; set; }
}

public class BackgroundCheckItem
{
    public bool? Value { get; set; }
    public DateTime? ChangeDateTimeUtc { get; set; }
    public string ChangeMadeBy { get; set; }
}

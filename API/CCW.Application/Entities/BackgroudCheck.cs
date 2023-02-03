namespace CCW.Application.Entities;


public class BackgroudCheck
{
    public ProofOfID ProofOfID { get; set; }
    public ProofOfResidency ProofOfResidency { get; set; }
    public NCICWantsWarrants NCICWantsWarrants { get; set; }
    public Locals Locals { get; set; }
    public Probations Probations { get; set; }
    public DMVRecord DMVRecord { get; set; }
    public AKSsChecked AKSsChecked { get; set; }
    public Coplink Coplink { get; set; }
    public TrafficCourtPortal TrafficCourtPortal { get; set; }
    public PropertyAssesor PropertyAssesor { get; set; }
    public VoterRegistration VoterRegistration { get; set; }
    public DOJApprovalLetter DOJApprovalLetter { get; set; }
    public CIINumber CIINumber { get; set; }
    public DOJ DOJ { get; set; }
    public FBI FBI { get; set; }
    public SR14 SR14 { get; set; }
    public FirearmsReg FirearmsReg { get; set; }
    public AllDearChiefLTRsRCRD AllDearChiefLTRsRCRD { get; set; }
    public SafetyCertificate SafetyCertificate { get; set; }
    public Restrictions Restrictions { get; set; }
}

public class ProofOfID
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class ProofOfResidency
{
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class NCICWantsWarrants
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class Locals
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class Probations
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class DMVRecord
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class AKSsChecked
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class Coplink
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class TrafficCourtPortal
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class PropertyAssesor
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class VoterRegistration
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class DOJApprovalLetter
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class CIINumber
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class DOJ
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class FBI
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class SR14
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class FirearmsReg
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class AllDearChiefLTRsRCRD
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class SafetyCertificate
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

    public class Restrictions
    {
        public bool? Value { get; set; }
        public DateTime? ChangeDateTimeUtc { get; set; }
        public string? ChangeMadeBy { get; set; }
    }

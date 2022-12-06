using CCW.Application.Enum;

namespace CCW.Application.Entities;

public class Application
{
    public Alias[]? Aliases { get; set; }
    public string? ApplicationType { get; set; }
    public Citizenship? Citizenship { get; set; }
    public Contact? Contact { get; set; }
    public Address? CurrentAddress { get; set; }
    public bool? DifferentMailing { get; set; }
    public bool? DifferentSpouseAddress { get; set; }
    public DOB? DOB { get; set; }
    public string? Employment { get; set; }
    public IdInfo? IdInfo { get; set; }
    public bool IsComplete { get; set; }
    public ImmigrantInformation? ImmigrantInformation { get; set; }
    public License? License { get; set; }
    public MailingAddress? MailingAddress { get; set; }
    public PersonalInfo? PersonalInfo { get; set; }
    public PhysicalAppearance? PhysicalAppearance { get; set; }
    public Address[]? PreviousAddresses { get; set; }
    public SpouseInformation? SpouseInformation { get; set; }
    public SpouseAddressInformation? SpouseAddressInformation { get; set; }
    public string UserEmail { get; set; }
    public Weapon[]? Weapons { get; set; }
    public WorkInformation? WorkInformation { get; set; }
    public QualifyingQuestions? QualifyingQuestions { get; set; }
    public int? CurrentStep { get; set; }
    public ApplicationStatus Status { get; set; }
    public bool? AppointmentStatus { get; set; }
    public DateTime? AppointmentDateTime { get; set; }
    public bool? ProofOfID { get; set; }
    public bool? ProofOfResidency { get; set; }
    public bool? NCICWantsWarrants { get; set; }
    public bool? Locals { get; set; }
    public bool? Probations { get; set; }
    public bool? DMVRecord { get; set; }
    public bool? AKSsChecked { get; set; }
    public bool? Coplink { get; set; }
    public bool? TrafficCourtPortal { get; set; }
    public bool? PropertyAssesor { get; set; }
    public bool? VoterRegistration { get; set; }
    public bool? DOJApprovalLetter { get; set; }
    public bool? CIINumber { get; set; }
    public bool? DOJ { get; set; }
    public bool? FBI { get; set; }
    public bool? SR14 { get; set; }
    public bool? FirearmsReg { get; set; }
    public bool? AllDearChiefLTRsRCRD { get; set; }
    public bool? SafetyCertificate { get; set; }
    public bool? Restrictions { get; set; }
    public string? Comments { get; set; }
    public string? OrderId { get; set; }
    public UploadedDocument[]? UploadedDocuments { get; set; }
}
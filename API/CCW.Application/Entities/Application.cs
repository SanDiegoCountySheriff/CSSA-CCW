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
    public PaymentStatus? PaymentStatus {get; set;}
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
    public DateTime? SubmittedToLicensingDateTime { get; set; }
    public DateTime? AppointmentDateTime { get; set; }
    public BackgroudCheck? BackgroudCheck { get; set; }
    public string? Comments { get; set; }
    public string? OrderId { get; set; }
    public UploadedDocument[]? UploadedDocuments { get; set; }
}


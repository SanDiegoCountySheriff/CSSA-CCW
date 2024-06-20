using CCW.Common.Enums;

namespace CCW.Common.Models;

public class UserApplication
{
    public Alias[] Aliases { get; set; }
    public ApplicationType ApplicationType { get; set; }
    public List<CharacterReference> CharacterReferences { get; set; }
    public Citizenship Citizenship { get; set; }
    public Contact Contact { get; set; }
    public Address CurrentAddress { get; set; }
    public Address ModifiedAddress { get; set; }
    public bool? ModifiedAddressComplete { get; set; }
    public DenialInfo DenialInfo { get; set; }
    public bool DifferentMailing { get; set; }
    public bool DifferentSpouseAddress { get; set; }
    public DOB DOB { get; set; }
    public string Employment { get; set; }
    public IdInfo IdInfo { get; set; }
    public bool IsComplete { get; set; }
    public bool IsUpdatingApplication { get; set; }
    public ImmigrantInformation ImmigrantInformation { get; set; }
    public License License { get; set; }
    public LiveScanInfo LiveScanInfo { get; set; }
    public MailingAddress MailingAddress { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PersonalInfo PersonalInfo { get; set; }
    public bool? ModifiedNameComplete { get; set; }
    public PhysicalAppearance PhysicalAppearance { get; set; }
    public Address[] PreviousAddresses { get; set; }
    public RevocationInfo RevocationInfo { get; set; }
    public SpouseInformation SpouseInformation { get; set; }
    public SpouseAddressInformation SpouseAddressInformation { get; set; }
    public string UserEmail { get; set; }
    public Weapon[] Weapons { get; set; }
    public Weapon[] ModifyDeleteWeapons { get; set; }
    public Weapon[] ModifyAddWeapons { get; set; }
    public bool? ModifiedWeaponComplete { get; set; }
    public WorkInformation WorkInformation { get; set; }
    public QualifyingQuestions QualifyingQuestions { get; set; }
    public LegacyQualifyingQuestions LegacyQualifyingQuestions { get; set; }
    public int CurrentStep { get; set; }
    public ApplicationStatus? OriginalStatus { get; set; }
    public ApplicationStatus Status { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public string AppointmentId { get; set; }
    public DateTimeOffset? SubmittedToLicensingDateTime { get; set; }
    public DateTimeOffset? AppointmentDateTime { get; set; }
    public string OrderId { get; set; }
    public UploadedDocument[] UploadedDocuments { get; set; }
    public UploadedDocument[] AdminUploadedDocuments { get; set; }
    public string CiiNumber { get; set; }
    public Cost Cost { get; set; }
    public bool FlaggedForCustomerReview { get; set; }
    public bool FlaggedForLicensingReview { get; set; }
    public Agreements Agreements { get; set; }
    public bool ReadyForInitialPayment { get; set; } = false;
    public bool ReadyForRenewalPayment { get; set; } = false;
    public bool ReadyForModificationPayment { get; set; } = false;
    public bool ReadyForIssuancePayment { get; set; } = false;
    public int ModificationNumber { get; set; } = 1;
    public int RenewalNumber { get; set; } = 0;
    public bool IsRenewingWithLegacyQuestions { get; set; } = false;
}

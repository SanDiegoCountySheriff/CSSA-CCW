namespace CCW.Application.Enum;

public enum ApplicationStatus
{
    None = 0,
    Incomplete = 1,
    Submitted = 2,
    ReadyForAppointment = 3,
    AppointmentComplete = 4,
    BackgroundInProgress = 5,
    ContingentlyApproved = 6,
    Approved = 7,
    PermitDelivered = 8,
    Suspended = 9,
    Revoked = 10,
    Cancelled = 11,
    Denied = 12,
    Withdrawn = 13,
    FlaggedForReview = 14,
    AppointmentNoShow = 15,
    ContingentlyDenied = 16,
}
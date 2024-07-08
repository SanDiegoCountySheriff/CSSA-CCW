using CCW.Common.Enums;
using Newtonsoft.Json;
using System;

namespace CCW.Common.Models;

public class SummarizedPermitApplication
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string FirstName { get; set; }
    public string Suffix { get; set; }
    public ApplicationStatus Status { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public string AppointmentId { get; set; }
    public List<PaymentHistory> PaymentHistory { get; set; }
    public bool Paid
    {
        get
        {
            if (PaymentHistory != null)
            {
                return PaymentHistory.Any(ph =>
                {
                    return ph.Successful == true &&
                    (ph.PaymentType == PaymentType.InitialStandard ||
                    ph.PaymentType == PaymentType.InitialJudicial ||
                    ph.PaymentType == PaymentType.InitialReserve);
                });
            }
            return false;
        }
    }
    public ApplicationType ApplicationType { get; set; }
    public bool IsComplete { get; set; }
    public bool IsUpdatingApplication { get; set; }
    public DateTimeOffset? AppointmentDateTime { get; set; }
    public string AssignedTo { get; set; }
    public bool FlaggedForLicensingReview { get; set; }
    public bool FlaggedForCustomerReview { get; set; }
    public string BirthDate { get; set; }
    public List<Alias> Aliases { get; set; }
}
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
    public string FirstName { get; set; }
    public ApplicationStatus Status { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public List<PaymentHistory> PaymentHistory { get; set; }
    public bool Paid
    {
        get
        {
            return PaymentHistory.Any(ph =>
            {
                return ph.Successful == true && 
                (ph.PaymentType == PaymentType.InitialStandard || 
                ph.PaymentType == PaymentType.InitialJudicial || 
                ph.PaymentType == PaymentType.InitialReserve);
            });
        }
    }
    public ApplicationType ApplicationType { get; set; }
    public bool IsComplete { get; set; }
    public bool IsUpdatingApplication { get; set; }
    public DateTime? AppointmentDateTime { get; set; }
    public string AssignedTo { get; set; }
    public bool FlaggedForLicensingReview { get; set; }
    public bool FlaggedForCustomerReview { get; set; }
}
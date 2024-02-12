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
    public string UserEmail { get; set; }
    public Address CurrentAddress { get; set; }
    public ApplicationStatus OriginalStatus { get; set; }
    public ApplicationStatus Status { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string ApplicationType { get; set; }
    public bool IsComplete { get; set; }
    public DOB DOB { get; set; }
    public DateTime? AppointmentDateTime { get; set; }
    public string UserId { get; set; }
    public string AssignedTo { get; set; }
    public bool FlaggedForLicensingReview { get; set; }
    public bool FlaggedForCustomerReview { get; set; }
}
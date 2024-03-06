using System.ComponentModel;

namespace CCW.Common.Enums;

public enum PaymentType
{
    [Description("CCW Application Initial Payment")]
    InitialStandard,
    [Description("CCW Application Initial Judicial Payment")]
    InitialJudicial,
    [Description("CCW Application Initial Reserve Payment")]
    InitialReserve,
    [Description("CCW Application Initial Employment Payment")]
    InitialEmployment,
    [Description("CCW Application Modification Payment")]
    ModificationStandard,
    [Description("CCW Application Modification Judicial Payment")]
    ModificationJudicial,
    [Description("CCW Application Modification Reserve Payment")]
    ModificationReserve,
    [Description("CCW Application Modification Employment Payment")]
    ModificationEmployment,
    [Description("CCW Application Renewal Payment")]
    RenewalStandard,
    [Description("CCW Application Renewal Judicial Payment")]
    RenewalJudicial,
    [Description("CCW Application Renewal Reserve Payment")]
    RenewalReserve,
    [Description("CCW Application Renewal Employment Payment")]
    RenewalEmployment,
}
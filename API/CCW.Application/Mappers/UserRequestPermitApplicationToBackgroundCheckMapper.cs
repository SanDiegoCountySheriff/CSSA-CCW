using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToBackgroundCheckMapper : IMapper<UserPermitApplicationRequestModel, BackgroudCheck>
{
    public BackgroudCheck Map(UserPermitApplicationRequestModel source)
    {
        if (source.Application.BackgroudCheck == null)
        {
            return new BackgroudCheck();
        }

        return new BackgroudCheck
        {
            ProofOfID = new ProofOfID
            {
                Value = source.Application.BackgroudCheck?.ProofOfID.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.ProofOfID.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.ProofOfID.ChangeMadeBy,
            },
            ProofOfResidency = new ProofOfResidency
            {
                Value = source.Application.BackgroudCheck?.ProofOfResidency.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.ProofOfResidency.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.ProofOfResidency.ChangeMadeBy,
            },
            NCICWantsWarrants = new NCICWantsWarrants
            {
                Value = source.Application.BackgroudCheck?.NCICWantsWarrants.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.NCICWantsWarrants.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.NCICWantsWarrants.ChangeMadeBy,
            },
            Locals = new Locals
            {
                Value = source.Application.BackgroudCheck?.Locals.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.Locals.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.Locals.ChangeMadeBy,
            },
            Probations = new Probations
            {
                Value = source.Application.BackgroudCheck?.Probations.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.Probations.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.Probations.ChangeMadeBy,
            },
            DMVRecord = new DMVRecord
            {
                Value = source.Application.BackgroudCheck?.DMVRecord.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.DMVRecord.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.DMVRecord.ChangeMadeBy,
            },
            AKSsChecked = new AKSsChecked
            {
                Value = source.Application.BackgroudCheck?.AKSsChecked.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.AKSsChecked.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.AKSsChecked.ChangeMadeBy,
            },
            Coplink = new Coplink
            {
                Value = source.Application.BackgroudCheck?.Coplink.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.Coplink.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.Coplink.ChangeMadeBy,
            },
            TrafficCourtPortal = new TrafficCourtPortal
            {
                Value = source.Application.BackgroudCheck?.TrafficCourtPortal.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.TrafficCourtPortal.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.TrafficCourtPortal.ChangeMadeBy,
            },
            PropertyAssesor = new PropertyAssesor
            {
                Value = source.Application.BackgroudCheck?.PropertyAssesor.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.PropertyAssesor.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.PropertyAssesor.ChangeMadeBy,
            },
            VoterRegistration = new VoterRegistration
            {
                Value = source.Application.BackgroudCheck?.VoterRegistration.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.VoterRegistration.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.VoterRegistration.ChangeMadeBy,
            },
            DOJApprovalLetter = new DOJApprovalLetter
            {
                Value = source.Application.BackgroudCheck?.DOJApprovalLetter.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.DOJApprovalLetter.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.DOJApprovalLetter.ChangeMadeBy,
            },
            CIINumber = new CIINumber
            {
                Value = source.Application.BackgroudCheck?.CIINumber.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.CIINumber.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.CIINumber.ChangeMadeBy,
            },
            DOJ = new DOJ
            {
                Value = source.Application.BackgroudCheck?.DOJ.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.DOJ.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.DOJ.ChangeMadeBy,
            },
            FBI = new FBI
            {
                Value = source.Application.BackgroudCheck?.FBI.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.FBI.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.FBI.ChangeMadeBy,
            },
            SR14 = new SR14
            {
                Value = source.Application.BackgroudCheck?.SR14.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.SR14.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.SR14.ChangeMadeBy,
            },
            FirearmsReg = new FirearmsReg
            {
                Value = source.Application.BackgroudCheck?.FirearmsReg.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.FirearmsReg.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.FirearmsReg.ChangeMadeBy,
            },
            AllDearChiefLTRsRCRD = new AllDearChiefLTRsRCRD
            {
                Value = source.Application.BackgroudCheck?.AllDearChiefLTRsRCRD.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.AllDearChiefLTRsRCRD.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.AllDearChiefLTRsRCRD.ChangeMadeBy,
            },
            SafetyCertificate = new SafetyCertificate
            {
                Value = source.Application.BackgroudCheck?.SafetyCertificate.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.SafetyCertificate.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.SafetyCertificate.ChangeMadeBy,
            },
            Restrictions = new Restrictions
            {
                Value = source.Application.BackgroudCheck?.Restrictions.Value,
                ChangeDateTimeUtc = source.Application.BackgroudCheck?.Restrictions.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroudCheck?.Restrictions.ChangeMadeBy,
            },
        };
    }
}

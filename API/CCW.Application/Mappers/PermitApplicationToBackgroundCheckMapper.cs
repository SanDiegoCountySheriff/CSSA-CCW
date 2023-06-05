using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToBackgroundCheckMapper : IMapper<PermitApplication, BackgroundCheck>
{
    public BackgroundCheck Map(PermitApplication source)
    {
        if (source.Application.BackgroundCheck == null)
        {
            return new BackgroundCheck(); 
        }

        return new BackgroundCheck
        {
            ProofOfID = new ProofOfID
            {
                Value = source.Application.BackgroundCheck?.ProofOfID.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.ProofOfID.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.ProofOfID.ChangeMadeBy,
            },
            ProofOfResidency = new ProofOfResidency
            {
                Value = source.Application.BackgroundCheck?.ProofOfResidency.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.ProofOfResidency.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.ProofOfResidency.ChangeMadeBy,
            },
            NCICWantsWarrants = new NCICWantsWarrants
            {
                Value = source.Application.BackgroundCheck?.NCICWantsWarrants.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.NCICWantsWarrants.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.NCICWantsWarrants.ChangeMadeBy,
            },
            Locals = new Locals
            {
                Value = source.Application.BackgroundCheck?.Locals.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.Locals.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.Locals.ChangeMadeBy,
            },
            Probations = new Probations
            {
                Value = source.Application.BackgroundCheck?.Probations.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.Probations.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.Probations.ChangeMadeBy,
            },
            DMVRecord = new DMVRecord
            {
                Value = source.Application.BackgroundCheck?.DMVRecord.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.DMVRecord.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.DMVRecord.ChangeMadeBy,
            },
            AKAsChecked = new AKAsChecked
            {
                Value = source.Application.BackgroundCheck?.AKAsChecked.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.AKAsChecked.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.AKAsChecked.ChangeMadeBy,
            },
            Coplink = new Coplink
            {
                Value = source.Application.BackgroundCheck?.Coplink.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.Coplink.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.Coplink.ChangeMadeBy,
            },
            TrafficCourtPortal = new TrafficCourtPortal
            {
                Value = source.Application.BackgroundCheck?.TrafficCourtPortal.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.TrafficCourtPortal.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.TrafficCourtPortal.ChangeMadeBy,
            },
            PropertyAssesor = new PropertyAssesor
            {
                Value = source.Application.BackgroundCheck?.PropertyAssesor.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.PropertyAssesor.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.PropertyAssesor.ChangeMadeBy,
            },
            VoterRegistration = new VoterRegistration
            {
                Value = source.Application.BackgroundCheck?.VoterRegistration.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.VoterRegistration.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.VoterRegistration.ChangeMadeBy,
            },
            DOJApprovalLetter = new DOJApprovalLetter
            {
                Value = source.Application.BackgroundCheck?.DOJApprovalLetter.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.DOJApprovalLetter.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.DOJApprovalLetter.ChangeMadeBy,
            },
            CIINumber = new CIINumber
            {
                Value = source.Application.BackgroundCheck?.CIINumber.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.CIINumber.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.CIINumber.ChangeMadeBy,
            },
            DOJ = new DOJ
            {
                Value = source.Application.BackgroundCheck?.DOJ.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.DOJ.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.DOJ.ChangeMadeBy,
            },
            FBI = new FBI
            {
                Value = source.Application.BackgroundCheck?.FBI.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.FBI.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.FBI.ChangeMadeBy,
            },
            SR14 = new SR14
            {
                Value = source.Application.BackgroundCheck?.SR14.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.SR14.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.SR14.ChangeMadeBy,
            },
            Firearms = new Firearms
            {
                Value = source.Application.BackgroundCheck?.Firearms.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.Firearms.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.Firearms.ChangeMadeBy,
            },
            SidLettersReceived = new SidLettersReceived
            {
                Value = source.Application.BackgroundCheck?.SidLettersReceived.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.SidLettersReceived.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.SidLettersReceived.ChangeMadeBy,
            },
            SafetyCertificate = new SafetyCertificate
            {
                Value = source.Application.BackgroundCheck?.SafetyCertificate.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.SafetyCertificate.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.SafetyCertificate.ChangeMadeBy,
            },
            Restrictions = new Restrictions
            {
                Value = source.Application.BackgroundCheck?.Restrictions.Value,
                ChangeDateTimeUtc = source.Application.BackgroundCheck?.Restrictions.ChangeDateTimeUtc,
                ChangeMadeBy = source.Application.BackgroundCheck?.Restrictions.ChangeMadeBy,
            },
        };
    }
}

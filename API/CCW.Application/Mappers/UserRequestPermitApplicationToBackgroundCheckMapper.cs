using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToBackgroundCheckMapper : IMapper<UserPermitApplicationRequestModel, BackgroudCheck>
{
    public BackgroudCheck Map(UserPermitApplicationRequestModel source)
    {
        return new BackgroudCheck
        {
            ProofOfID = source.Application.BackgroudCheck?.ProofOfID,
            ProofOfResidency = source.Application.BackgroudCheck?.ProofOfResidency,
            NCICWantsWarrants = source.Application.BackgroudCheck?.NCICWantsWarrants,
            Locals = source.Application.BackgroudCheck?.Locals,
            Probations = source.Application.BackgroudCheck?.Probations,
            DMVRecord = source.Application.BackgroudCheck?.DMVRecord,
            AKSsChecked = source.Application.BackgroudCheck?.AKSsChecked,
            Coplink = source.Application.BackgroudCheck?.Coplink,
            TrafficCourtPortal = source.Application.BackgroudCheck?.TrafficCourtPortal,
            PropertyAssesor = source.Application.BackgroudCheck?.PropertyAssesor,
            VoterRegistration = source.Application.BackgroudCheck?.VoterRegistration,
            DOJApprovalLetter = source.Application.BackgroudCheck?.DOJApprovalLetter,
            CIINumber = source.Application.BackgroudCheck?.CIINumber,
            DOJ = source.Application.BackgroudCheck?.DOJ,
            FBI = source.Application.BackgroudCheck?.FBI,
            SR14 = source.Application.BackgroudCheck?.SR14,
            FirearmsReg = source.Application.BackgroudCheck?.FirearmsReg,
            AllDearChiefLTRsRCRD = source.Application.BackgroudCheck?.AllDearChiefLTRsRCRD,
            SafetyCertificate = source.Application.BackgroudCheck?.SafetyCertificate,
            Restrictions = source.Application.BackgroudCheck?.Restrictions,
        };
    }
}

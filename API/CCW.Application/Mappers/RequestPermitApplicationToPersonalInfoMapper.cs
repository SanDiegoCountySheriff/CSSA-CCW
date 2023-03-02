using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToPersonalInfoMapper : IMapper<PermitApplicationRequestModel, PersonalInfo>
{
    public PersonalInfo Map(PermitApplicationRequestModel source)
    {
        return source.Application.PersonalInfo == null ? new PersonalInfo() :
            new PersonalInfo
            {
                LastName = source.Application.PersonalInfo.LastName,
                FirstName = source.Application.PersonalInfo.FirstName,
                MiddleName = source.Application.PersonalInfo.MiddleName,
                NoMiddleName = source.Application.PersonalInfo.NoMiddleName,
                MaidenName = source.Application.PersonalInfo.MaidenName,
                Suffix = source.Application.PersonalInfo.Suffix,
                Ssn = source.Application.PersonalInfo.Ssn,
                MaritalStatus = source.Application.PersonalInfo.MaritalStatus,
            };
    }
}

using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToPersonalInfoMapper : IMapper<UserPermitApplicationRequestModel, PersonalInfo>
{
    public PersonalInfo Map(UserPermitApplicationRequestModel source)
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

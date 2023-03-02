using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToPersonalInfoMapper : IMapper<PermitApplication, PersonalInfo>
{
    public PersonalInfo Map(PermitApplication source)
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
                Ssn = (!string.IsNullOrEmpty(source.Application.PersonalInfo.Ssn)) ?
                "XXX-XX-" + source.Application.PersonalInfo.Ssn.Substring(source.Application.PersonalInfo.Ssn.Length - 4, 4) :
                "",
                MaritalStatus = source.Application.PersonalInfo.MaritalStatus,
            };
    }
}

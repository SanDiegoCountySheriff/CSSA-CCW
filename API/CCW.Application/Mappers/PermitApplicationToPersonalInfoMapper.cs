using CCW.Application.Entities;
using System.Runtime.Intrinsics.X86;

namespace CCW.Application.Mappers;

public class PermitApplicationToPersonalInfoMapper : IMapper<PermitApplication, PersonalInfo>
{
    public PersonalInfo Map(PermitApplication source)
    {
        return new PersonalInfo
        {
            LastName = source.Application.PersonalInfo.LastName,
            FirstName = source.Application.PersonalInfo.FirstName,
            MiddleName = source.Application.PersonalInfo.MiddleName,
            NoMiddleName = source.Application.PersonalInfo.NoMiddleName,
            MaidenName = source.Application.PersonalInfo.MaidenName,
            Suffix = source.Application.PersonalInfo.Suffix,
            Ssn = "XXX-XX-" + source.Application.PersonalInfo.Ssn.Substring(source.Application.PersonalInfo.Ssn.Length - 4, 4), //source.Application.PersonalInfo.Ssn,
            MaritalStatus = source.Application.PersonalInfo.MaritalStatus,
        };
    }
}

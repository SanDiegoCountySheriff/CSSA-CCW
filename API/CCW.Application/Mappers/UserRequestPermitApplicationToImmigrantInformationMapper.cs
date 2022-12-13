using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToImmigrantInformationMapper : IMapper<UserPermitApplicationRequestModel, ImmigrantInformation>
{
    public ImmigrantInformation Map(UserPermitApplicationRequestModel source)
    {
        return new ImmigrantInformation
        {
            CountryOfBirth = source.Application.ImmigrantInformation?.CountryOfBirth,
            CountryOfCitizenship = source.Application.ImmigrantInformation?.CountryOfCitizenship,
            ImmigrantAlien = source.Application.ImmigrantInformation?.ImmigrantAlien,
            NonImmigrantAlien = source.Application.ImmigrantInformation?.NonImmigrantAlien,
        };
    }
}
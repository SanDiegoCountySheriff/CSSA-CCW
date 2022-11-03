using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToImmigrantInformationMapper : IMapper<PermitApplication, ImmigrantInformation>
{
    public ImmigrantInformation Map(PermitApplication source)
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
using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToImmigrantInformationMapper : IMapper<PermitApplicationRequestModel, ImmigrantInformation>
{
    public ImmigrantInformation Map(PermitApplicationRequestModel source)
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
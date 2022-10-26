using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToDOBMapper : IMapper<PermitApplicationRequestModel, DOB>
{
    public DOB Map(PermitApplicationRequestModel source)
    {
        return new DOB
        {
            BirthDate = source.Application.DOB.BirthDate,
            BirthCity = source.Application.DOB.BirthCity,
            BirthState = source.Application.DOB.BirthState,
            BirthCountry = source.Application.DOB.BirthCountry,
        };
    }
}

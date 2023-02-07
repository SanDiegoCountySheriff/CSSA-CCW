using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToDOBMapper : IMapper<PermitApplication, DOB>
{
    public DOB Map(PermitApplication source)
    {
        return source.Application.DOB == null ? new DOB() :
            new DOB
            {
                BirthDate = source.Application.DOB.BirthDate,
                BirthCity = source.Application.DOB.BirthCity,
                BirthState = source.Application.DOB.BirthState,
                BirthCountry = source.Application.DOB.BirthCountry,
            };
    }
}

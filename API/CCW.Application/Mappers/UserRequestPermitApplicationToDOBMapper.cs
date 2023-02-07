using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToDOBMapper : IMapper<UserPermitApplicationRequestModel, DOB>
{
    public DOB Map(UserPermitApplicationRequestModel source)
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

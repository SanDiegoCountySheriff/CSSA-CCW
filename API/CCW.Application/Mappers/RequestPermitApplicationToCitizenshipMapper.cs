using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToCitizenshipMapper : IMapper<PermitApplicationRequestModel, Citizenship>
{
    public Citizenship Map(PermitApplicationRequestModel source)
    {
        return source.Application.Citizenship == null ? new Citizenship() :
            new Citizenship
            {
                Citizen = source.Application.Citizenship.Citizen,
                MilitaryStatus = source.Application.Citizenship.MilitaryStatus,
            };
    }
}

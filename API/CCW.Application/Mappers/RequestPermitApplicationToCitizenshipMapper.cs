using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class RequestPermitApplicationToCitizenshipMapper : IMapper<PermitApplicationRequestModel, Citizenship>
    {
        public Citizenship Map(PermitApplicationRequestModel source)
        {
            return new Citizenship
            {
                Citizen = source.Application.Citizenship.Citizen,
                MilitaryStatus = source.Application.Citizenship.MilitaryStatus,
            };
        }
    }
}

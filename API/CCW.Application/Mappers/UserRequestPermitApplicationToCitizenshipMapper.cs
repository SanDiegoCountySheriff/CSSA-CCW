using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class UserRequestPermitApplicationToCitizenshipMapper : IMapper<UserPermitApplicationRequestModel, Citizenship>
    {
        public Citizenship Map(UserPermitApplicationRequestModel source)
        {
            return new Citizenship
            {
                Citizen = source.Application.Citizenship.Citizen,
                MilitaryStatus = source.Application.Citizenship.MilitaryStatus,
            };
        }
    }
}

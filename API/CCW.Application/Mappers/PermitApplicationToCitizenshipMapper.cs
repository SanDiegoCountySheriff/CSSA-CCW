using CCW.Application.Entities;

namespace CCW.Application.Mappers
{
    public class PermitApplicationToCitizenshipMapper : IMapper<PermitApplication, Citizenship>
    {
        public Citizenship Map(PermitApplication source)
        {
            return new Citizenship
            {
                Citizen = source.Application.Citizenship.Citizen,
                MilitaryStatus = source.Application.Citizenship.MilitaryStatus,
            };
        }
    }
}

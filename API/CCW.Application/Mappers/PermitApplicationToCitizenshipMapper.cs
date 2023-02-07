using CCW.Application.Entities;

namespace CCW.Application.Mappers
{
    public class PermitApplicationToCitizenshipMapper : IMapper<PermitApplication, Citizenship>
    {
        public Citizenship Map(PermitApplication source)
        {
            return source.Application.Citizenship == null ? new Citizenship() : 
                new Citizenship
            {
                Citizen = source.Application.Citizenship.Citizen,
                MilitaryStatus = source.Application.Citizenship.MilitaryStatus,
            };
        }
    }
}

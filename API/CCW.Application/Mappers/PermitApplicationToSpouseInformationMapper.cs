using CCW.Application.Entities;

namespace CCW.Application.Mappers
{
    public class PermitApplicationToSpouseInformationMapper : IMapper<PermitApplication, SpouseInformation>
    {
        public SpouseInformation Map(PermitApplication source)
        {
            return new SpouseInformation
            {
                FirstName = source.Application.SpouseInformation.FirstName,
                LastName = source.Application.SpouseInformation.LastName,
                MaidenName = source.Application.SpouseInformation.MaidenName,
                MiddleName = source.Application.SpouseInformation.MiddleName,
                PhoneNumber = source.Application.SpouseInformation.PhoneNumber,
            };
        }
    }
}

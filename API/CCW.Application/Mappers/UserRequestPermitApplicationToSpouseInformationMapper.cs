using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class UserRequestPermitApplicationToSpouseInformationMapper : IMapper<UserPermitApplicationRequestModel, SpouseInformation>
    {
        public SpouseInformation Map(UserPermitApplicationRequestModel source)
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

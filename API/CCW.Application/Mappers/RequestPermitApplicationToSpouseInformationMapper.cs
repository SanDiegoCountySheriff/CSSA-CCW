using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class RequestPermitApplicationToSpouseInformationMapper : IMapper<PermitApplicationRequestModel, SpouseInformation>
    {
        public SpouseInformation Map(PermitApplicationRequestModel source)
        {
            return new SpouseInformation
            {
                FirstName = source.Application.SpouseInformation.FirstName,
                LastName = source.Application.SpouseInformation.LastName,
                MaidenName = source.Application.SpouseInformation.MaidenName,
                MiddleName = source.Application.SpouseInformation.MiddleName,
            };
        }
    }
}

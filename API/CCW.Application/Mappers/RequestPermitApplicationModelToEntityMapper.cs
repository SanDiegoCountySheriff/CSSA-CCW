using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class RequestPermitApplicationModelToEntityMapper : IMapper<bool, PermitApplicationRequestModel, PermitApplication>
    {
        private readonly IMapper<PermitApplicationRequestModel, Entities.Application> _applicationMapper;

        public RequestPermitApplicationModelToEntityMapper(IMapper<PermitApplicationRequestModel, Entities.Application> applicationMapper)
        {
            _applicationMapper = applicationMapper;
        }

        public PermitApplication Map(bool isNewApplication, PermitApplicationRequestModel source)
        {
            return new PermitApplication
            {
                Application = _applicationMapper.Map(source),
                Id = isNewApplication ? Guid.NewGuid() : source.Id,
            };
        }
    }
}

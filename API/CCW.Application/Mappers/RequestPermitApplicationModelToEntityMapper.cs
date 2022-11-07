using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class RequestPermitApplicationModelToEntityMapper : IMapper<bool, PermitApplicationRequestModel, PermitApplication>
    {
        private readonly IMapper<PermitApplicationRequestModel, Entities.Application> _applicationMapper;
        private readonly IMapper<PermitApplicationRequestModel, History[]> _historyMapper;

        public RequestPermitApplicationModelToEntityMapper(
            IMapper<PermitApplicationRequestModel, Entities.Application> applicationMapper,
            IMapper<PermitApplicationRequestModel, History[]> historyMapper)
        {
            _applicationMapper = applicationMapper;
            _historyMapper = historyMapper;
        }

        public PermitApplication Map(bool isNewApplication, PermitApplicationRequestModel source)
        {
            return new PermitApplication
            {
                Application = _applicationMapper.Map(source),
                Id = isNewApplication ? Guid.NewGuid() : source.Id,
                History = _historyMapper.Map(source),
            };
        }
    }
}

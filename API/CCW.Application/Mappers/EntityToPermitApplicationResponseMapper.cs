using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class EntityToPermitApplicationResponseMapper : IMapper<PermitApplication, PermitApplicationResponseModel>
{
    private readonly IMapper<PermitApplication, Entities.Application> _applicationMapper;

    public EntityToPermitApplicationResponseMapper(IMapper<PermitApplication, Entities.Application> applicationMapper)
    {
        _applicationMapper = applicationMapper;
    }
    public PermitApplicationResponseModel Map(PermitApplication source)
    {
        return new PermitApplicationResponseModel
        {
            Application = _applicationMapper.Map(source),
            Id = source.Id,
        };
    }
}

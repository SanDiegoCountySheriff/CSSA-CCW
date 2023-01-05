using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class EntityToUserPermitApplicationResponseMapper : IMapper<PermitApplication, UserPermitApplicationResponseModel>
{
    private readonly IMapper<PermitApplication, UserApplication> _applicationMapper;

    public EntityToUserPermitApplicationResponseMapper(
        IMapper<PermitApplication, UserApplication> applicationMapper)
    {
        _applicationMapper = applicationMapper;
    }

    public UserPermitApplicationResponseModel Map(PermitApplication source)
    {
        return new UserPermitApplicationResponseModel
        {
            Application = _applicationMapper.Map(source),
            Id = source.Id,
            UserId = source.UserId,
        };
    }
}

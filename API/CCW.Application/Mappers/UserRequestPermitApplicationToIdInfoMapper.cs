using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToIdInfoMapper : IMapper<UserPermitApplicationRequestModel, IdInfo>
{
    public IdInfo Map(UserPermitApplicationRequestModel source)
    {
        return source.Application.IdInfo == null ? new IdInfo() :
            new IdInfo
            {
                IdNumber = source.Application.IdInfo.IdNumber,
                IssuingState = source.Application.IdInfo.IssuingState,
            };
    }
}

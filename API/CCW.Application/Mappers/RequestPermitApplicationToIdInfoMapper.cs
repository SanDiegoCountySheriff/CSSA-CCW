using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToIdInfoMapper : IMapper<PermitApplicationRequestModel, IdInfo>
{
    public IdInfo Map(PermitApplicationRequestModel source)
    {
        return source.Application.IdInfo == null ? new IdInfo() :
            new IdInfo
        {
            IdNumber = source.Application.IdInfo.IdNumber,
            IssuingState = source.Application.IdInfo.IssuingState,
        };
    }
}

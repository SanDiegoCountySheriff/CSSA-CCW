using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToIdInfoMapper : IMapper<PermitApplication, IdInfo>
{
    public IdInfo Map(PermitApplication source)
    {
        return source.Application.IdInfo == null ? new IdInfo() :
            new IdInfo
            {
                IdNumber = source.Application.IdInfo.IdNumber,
                IssuingState = source.Application.IdInfo.IssuingState,
            };
    }
}

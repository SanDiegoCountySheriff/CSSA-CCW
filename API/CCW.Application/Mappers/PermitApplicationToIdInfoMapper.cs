using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToIdInfoMapper : IMapper<PermitApplication, IdInfo>
{
    public IdInfo Map(PermitApplication source)
    {
        return new IdInfo
        {
            IdNumber = source.Application.IdInfo.IdNumber,
            IssuingState = source.Application.IdInfo.IssuingState,
        };
    }
}

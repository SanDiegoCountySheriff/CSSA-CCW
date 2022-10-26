using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToPhysicalAppearanceMapper : IMapper<PermitApplicationRequestModel, PhysicalAppearance>
{
    public PhysicalAppearance Map(PermitApplicationRequestModel source)
    {
        return new PhysicalAppearance
        {
            Gender = source.Application.PhysicalAppearance.Gender,
            HeightFeet = source.Application.PhysicalAppearance.HeightFeet,
            HeightInch = source.Application.PhysicalAppearance.HeightInch,
            Weight = source.Application.PhysicalAppearance.Weight,
            HairColor = source.Application.PhysicalAppearance.HairColor,
            EyeColor = source.Application.PhysicalAppearance.EyeColor,
            PhysicalDesc = source.Application.PhysicalAppearance.PhysicalDesc,
        };
    }
}

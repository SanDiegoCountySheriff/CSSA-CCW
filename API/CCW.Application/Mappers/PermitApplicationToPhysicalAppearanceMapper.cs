using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToPhysicalAppearanceMapper : IMapper<PermitApplication, PhysicalAppearance>
{
    public PhysicalAppearance Map(PermitApplication source)
    {
        return new PhysicalAppearance
        {
            Gender = source.Application.PhysicalAppearance?.Gender,
            HeightFeet = source.Application.PhysicalAppearance?.HeightFeet,
            HeightInch = source.Application.PhysicalAppearance?.HeightInch,
            Weight = source.Application.PhysicalAppearance?.Weight,
            HairColor = source.Application.PhysicalAppearance?.HairColor,
            EyeColor = source.Application.PhysicalAppearance?.EyeColor,
            PhysicalDesc = source.Application.PhysicalAppearance?.PhysicalDesc,
        };
    }
}

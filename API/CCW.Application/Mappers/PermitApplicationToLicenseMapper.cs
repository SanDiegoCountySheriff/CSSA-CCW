using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToLicenseMapper : IMapper<PermitApplication, License>
{
    public License Map(PermitApplication source)
    {
        return new License
        {
            ExpirationDate = source.Application.License.ExpirationDate,
            IssuingCounty = source.Application.License.IssuingCounty,
            PermitNumber = source.Application.License.PermitNumber,
        };
    }
}

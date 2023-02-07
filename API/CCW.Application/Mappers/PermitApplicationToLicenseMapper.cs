using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToLicenseMapper : IMapper<PermitApplication, License>
{
    public License Map(PermitApplication source)
    {
        return source.Application.License == null ? new License() :
                new License
                {
                    ExpirationDate = source.Application.License.ExpirationDate,
                    IssuingCounty = source.Application.License.IssuingCounty,
                    PermitNumber = source.Application.License.PermitNumber,
                    IssueDate = source.Application.License.IssueDate,
                };
    }
}

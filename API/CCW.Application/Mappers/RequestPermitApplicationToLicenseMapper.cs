using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToLicenseMapper : IMapper<PermitApplicationRequestModel, License>
{
    public License Map(PermitApplicationRequestModel source)
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

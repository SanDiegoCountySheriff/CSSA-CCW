using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToSpouseAddressInformationMapper : IMapper<PermitApplicationRequestModel, SpouseAddressInformation>
{
    public SpouseAddressInformation Map(PermitApplicationRequestModel source)
    {
        return source.Application.SpouseAddressInformation == null ? new SpouseAddressInformation() :
            new SpouseAddressInformation
            {
                AddressLine1 = source.Application.SpouseAddressInformation.AddressLine1,
                AddressLine2 = source.Application.SpouseAddressInformation.AddressLine2,
                City = source.Application.SpouseAddressInformation.City,
                County = source.Application.SpouseAddressInformation.County,
                State = source.Application.SpouseAddressInformation.State,
                Zip = source.Application.SpouseAddressInformation.Zip,
                Country = source.Application.SpouseAddressInformation.Country,
            };
    }
}

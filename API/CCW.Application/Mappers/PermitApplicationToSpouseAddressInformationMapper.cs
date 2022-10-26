using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToSpouseAddressInformationMapper : IMapper<PermitApplication, SpouseAddressInformation>
{
    public SpouseAddressInformation Map(PermitApplication source)
    {
        return new SpouseAddressInformation
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

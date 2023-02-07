using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToAddressMapper : IMapper<PermitApplication, Address>
{
    public Address Map(PermitApplication source)
    {
        return source.Application.CurrentAddress == null ? new Address() :
            new Address
            {
                AddressLine1 = source.Application.CurrentAddress.AddressLine1,
                AddressLine2 = source.Application.CurrentAddress.AddressLine2,
                City = source.Application.CurrentAddress.City,
                County = source.Application.CurrentAddress.County,
                State = source.Application.CurrentAddress.State,
                Zip = source.Application.CurrentAddress.Zip,
                Country = source.Application.CurrentAddress.Country,
            };
    }
}
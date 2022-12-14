using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

    public class RequestPermitApplicationToAddressMapper : IMapper<PermitApplicationRequestModel, Address>
    {
        public Address Map(PermitApplicationRequestModel source)
        {
            return new Address
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


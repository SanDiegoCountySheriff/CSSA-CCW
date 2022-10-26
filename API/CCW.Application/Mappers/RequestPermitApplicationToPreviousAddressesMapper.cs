using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class RequestPermitApplicationToPreviousAddressesMapper : IMapper<PermitApplicationRequestModel, Address[]>
    {
        public Address[] Map(PermitApplicationRequestModel source)
        {
            if (source.Application.Aliases != null)
            {
                int count = source.Application.PreviousAddresses.Length;
                var newItem = new Address[count];
                for (int i = 0; i < count; i++)
                {
                    newItem[i] = MapAlias(source.Application.PreviousAddresses[i], new Address());
                }

                return newItem;
            }

            return new Address[0];
        }
        private static Address MapAlias(Address uiAddress, Address dbAddress)
        {
            dbAddress.AddressLine1 = uiAddress.AddressLine1;
            dbAddress.AddressLine2 = uiAddress.AddressLine2;
            dbAddress.City = uiAddress.City;
            dbAddress.State = uiAddress.State;
            dbAddress.Zip = uiAddress.Zip;
            dbAddress.County = uiAddress.County;
            dbAddress.Country = uiAddress.Country;

            return dbAddress;
        }

    }
}

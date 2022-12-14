using CCW.Application.Entities;

namespace CCW.Application.Mappers
{
    public class PermitApplicationToPreviousAddressesMapper : IMapper<PermitApplication, Address[]>
    {
        public Address[] Map(PermitApplication source)
        {
            if (source.Application.PreviousAddresses != null)
            {
                int count = source.Application.PreviousAddresses.Length;
                var newItem = new Address[count];
                for (int i = 0; i < count; i++)
                {
                    newItem[i] = MapAlias(source.Application.PreviousAddresses[i], new Address());
                }

                return newItem;
            }

            return Array.Empty<Address>();
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

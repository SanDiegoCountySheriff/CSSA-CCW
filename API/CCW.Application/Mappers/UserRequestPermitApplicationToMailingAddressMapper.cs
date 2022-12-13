using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class UserRequestPermitApplicationToMailingAddressMapper : IMapper<UserPermitApplicationRequestModel, MailingAddress?>
    {
        public MailingAddress? Map(UserPermitApplicationRequestModel source)
        {
            if (source.Application.MailingAddress != null)
            {
                return new MailingAddress
                {
                    AddressLine1 = source.Application.MailingAddress.AddressLine1,
                    AddressLine2 = source.Application.MailingAddress.AddressLine2,
                    City = source.Application.MailingAddress.City,
                    County = source.Application.MailingAddress.County,
                    State = source.Application.MailingAddress.State,
                    Zip = source.Application.MailingAddress.Zip,
                    Country = source.Application.MailingAddress.Country,
                };
            }

            return null;
        }
    }
}

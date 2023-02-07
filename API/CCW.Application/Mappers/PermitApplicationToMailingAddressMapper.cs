using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToMailingAddressMapper : IMapper<PermitApplication, MailingAddress?>
{
    public MailingAddress? Map(PermitApplication source)
    {
        return (source.Application.MailingAddress == null)
            ? new MailingAddress()
            : new MailingAddress
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
}

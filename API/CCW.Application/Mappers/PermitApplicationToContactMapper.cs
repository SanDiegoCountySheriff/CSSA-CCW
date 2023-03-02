using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToContactMapper : IMapper<PermitApplication, Contact>
{
    public Contact Map(PermitApplication source)
    {
        return source.Application.Contact == null ? new Contact() :
            new Contact
            {
                PrimaryPhoneNumber = source.Application.Contact.PrimaryPhoneNumber,
                CellPhoneNumber = source.Application.Contact.CellPhoneNumber,
                WorkPhoneNumber = source.Application.Contact.WorkPhoneNumber,
                FaxPhoneNumber = source.Application.Contact.FaxPhoneNumber,
                TextMessageUpdates = source.Application.Contact.TextMessageUpdates,
            };
    }
}

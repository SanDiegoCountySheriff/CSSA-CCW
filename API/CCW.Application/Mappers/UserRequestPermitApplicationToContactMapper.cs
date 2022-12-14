using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToContactMapper : IMapper<UserPermitApplicationRequestModel, Contact>
{
    public Contact Map(UserPermitApplicationRequestModel source)
    {
        return new Contact
        {
            PrimaryPhoneNumber = source.Application.Contact.PrimaryPhoneNumber,
            CellPhoneNumber = source.Application.Contact.CellPhoneNumber,
            WorkPhoneNumber = source.Application.Contact.WorkPhoneNumber,
            FaxPhoneNumber = source.Application.Contact.FaxPhoneNumber,
            TextMessageUpdates = source.Application.Contact.TextMessageUpdates,
        };
    }
}

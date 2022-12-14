using CCW.Application.Entities;
using CCW.Application.Enum;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class EntityToSummarizedPermitApplicationModelMapper : IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel>
{
    public SummarizedPermitApplicationResponseModel Map(SummarizedPermitApplication source)
    {
        var address = new Address();

        if (source.CurrentAddress != null)
        {
            address = new Address
            {
                AddressLine1 = source.CurrentAddress.AddressLine1,
                AddressLine2 = source.CurrentAddress.AddressLine2,
                City = source.CurrentAddress.City,
                County = source.CurrentAddress.County,
                State = source.CurrentAddress.State,
                Zip = source.CurrentAddress.Zip,
                Country = source.CurrentAddress.Country,
            };
        }

        var dob = new DOB();

        if (source.DOB != null)
        {
            dob.BirthCity = source.DOB.BirthCity;
            dob.BirthCountry = source.DOB.BirthCountry;
            dob.BirthDate = source.DOB.BirthDate;
            dob.BirthState = source.DOB.BirthState;
        }

        return new SummarizedPermitApplicationResponseModel
        {
            FirstName = source.FirstName,
            LastName = source.LastName,
            Address = address,
            Status = source.Status != null ? (int)source.Status : 0,
            ApplicationID = source.id,
            AppointmentStatus = source.AppointmentStatus,
            Email = source.UserEmail,
            OrderID = source.OrderId,
            IsComplete = source.IsComplete,
            DOB = dob,
            AppointmentDateTime = source.AppointmentDateTime,
            ApplicationType = source.ApplicationType,
            UserId = source.UserId
        };
    }
}
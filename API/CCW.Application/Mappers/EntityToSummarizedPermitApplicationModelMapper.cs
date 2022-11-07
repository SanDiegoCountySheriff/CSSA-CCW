﻿using CCW.Application.Entities;
using CCW.Application.Enum;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class EntityToSummarizedPermitApplicationModelMapper : IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel>
{
    public SummarizedPermitApplicationResponseModel Map(SummarizedPermitApplication source)
    {
        return new SummarizedPermitApplicationResponseModel
        {
            Name = source.FirstName + source.LastName,
            Address = new Address
            {
                AddressLine1 = source.CurrentAddress.AddressLine1,
                AddressLine2 = source.CurrentAddress.AddressLine2,
                City = source.CurrentAddress.City,
                County = source.CurrentAddress.County,
                State = source.CurrentAddress.State,
                Zip = source.CurrentAddress.Zip,
                Country = source.CurrentAddress.Country,
            },
            Status = ApplicationStatus.None, //source.Status,
            ApplicationID = source.id,
            AppointmentStatus = AppointmentStatus.Complete, //source.AppointmentStatus,
            Email = source.UserEmail,
            OrderID = source.OrderId,
        };
    }
}

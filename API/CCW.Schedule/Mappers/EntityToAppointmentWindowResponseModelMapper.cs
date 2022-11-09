using CCW.Schedule.Entities;
using CCW.Schedule.Models;

namespace CCW.Schedule.Mappers;

public class EntityToAppointmentWindowResponseModelMapper : IMapper<AppointmentWindow, AppointmentWindowResponseModel>
{
    public AppointmentWindowResponseModel Map(AppointmentWindow source)
    {
        return new AppointmentWindowResponseModel
        {
            ApplicantId = source.ApplicantId,
            Id = source.Id,
            Start = source.Start,
            End = source.End,
            Permit = source.Permit,
            Name = source.Name,
            Payment = source.Payment,
            Status = source.Status,
            IsManuallyCreated = source.IsManuallyCreated,
        };
    }
}
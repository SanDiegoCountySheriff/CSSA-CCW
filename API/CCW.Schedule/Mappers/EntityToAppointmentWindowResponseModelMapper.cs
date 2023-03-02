using CCW.Schedule.Entities;
using CCW.Schedule.Models;

namespace CCW.Schedule.Mappers;

public class EntityToAppointmentWindowResponseModelMapper : IMapper<AppointmentWindow, AppointmentWindowResponseModel>
{
    public AppointmentWindowResponseModel Map(AppointmentWindow source)
    {
        return new AppointmentWindowResponseModel
        {
            ApplicationId = source.ApplicationId,
            Id = source.Id.ToString(),
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
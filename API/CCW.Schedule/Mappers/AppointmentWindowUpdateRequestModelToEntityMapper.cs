using CCW.Schedule.Entities;
using CCW.Schedule.Models;

namespace CCW.Schedule.Mappers;

public class AppointmentWindowUpdateRequestModelToEntityMapper : IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow>
{
    public AppointmentWindow Map(AppointmentWindowUpdateRequestModel source)
    {
        return new AppointmentWindow
        {
            ApplicationId = source.ApplicationId == "" ? null : source.ApplicationId,
            Id = new Guid(source.Id),
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
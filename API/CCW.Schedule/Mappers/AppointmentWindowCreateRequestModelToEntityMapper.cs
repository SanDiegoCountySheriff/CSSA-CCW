using CCW.Schedule.Entities;
using CCW.Schedule.Models;

namespace CCW.Schedule.Mappers;

public class AppointmentWindowCreateRequestModelToEntityMapper : IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow>
{
    public AppointmentWindow Map(AppointmentWindowCreateRequestModel source)
    {
        return new AppointmentWindow
        {
            ApplicationId = source.ApplicationId == "" ? null : source.ApplicationId,
            Id = Guid.NewGuid(),
            Start = source.Start.ToString(Constants.DateTimeFormat),
            End = source.End.ToString(Constants.DateTimeFormat),
            Permit = source.Permit,
            Name = source.Name,
            Payment = source.Payment,
            Status = source.Status,
            IsManuallyCreated = source.IsManuallyCreated,
        };
    }
}
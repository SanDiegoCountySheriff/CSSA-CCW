using CCW.Schedule.Entities;
using CCW.Schedule.Models;

namespace CCW.Schedule.Mappers;

public class AppointmentManagementRequestModelToEntityMapper : IMapper<AppointmentManagementRequestModel, AppointmentManagement>
{
    public AppointmentManagement Map(AppointmentManagementRequestModel source)
    {
        return new AppointmentManagement()
        {
            DaysOfTheWeek = source.DaysOfTheWeek,
            FirstAppointmentStartTime = source.FirstAppointmentStartTime,
            LastAppointmentStartTime = source.LastAppointmentStartTime,
            NumberOfSlotsPerAppointment = source.NumberOfSlotsPerAppointment,
            NumberOfWeeksToCreate = source.NumberOfWeeksToCreate,
            AppointmentLength = source.AppointmentLength,
            BreakLength = source.BreakLength,
            BreakStartTime = source.BreakStartTime,
        };
    }
}


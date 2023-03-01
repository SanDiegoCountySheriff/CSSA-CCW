using CCW.Schedule.Entities;

namespace CCW.Schedule.Services;

public interface ICosmosDbService
{
    Task<AppointmentWindow> GetAsync(string applicationId, CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> ResetApplicantAppointmentsAsync(string applicationId, CancellationToken cancellationToken);
    Task<AppointmentWindow> GetAppointmentByIdAsync(string appointmentId, CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> GetAvailableTimesAsync(CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> GetAvailableSlotByDateTime(DateTime startTime, CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> GetAllBookedAppointmentsAsync(CancellationToken cancellationToken);
    Task<AppointmentWindow> AddAsync(AppointmentWindow appointment, CancellationToken cancellationToken);
    Task AddAvailableTimesAsync(List<AppointmentWindow> appointments, CancellationToken cancellationToken);
    Task UpdateAsync(AppointmentWindow appointment, CancellationToken cancellationToken);
    Task AddDeleteTimeSlotsAsync(List<AppointmentWindow> appointments, bool isCreateAction,
        CancellationToken cancellationToken);
    Task DeleteAsync(string appointmentId, CancellationToken cancellationToken);
}
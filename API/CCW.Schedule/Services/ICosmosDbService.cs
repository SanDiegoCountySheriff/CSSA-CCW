using CCW.Schedule.Entities;

namespace CCW.Schedule.Services;

public interface ICosmosDbService
{
    Task<AppointmentWindow> GetAsync(string applicationId, CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> GetAvailableTimesAsync(CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> GetAllBookedAppointmentsAsync(CancellationToken cancellationToken);
    Task<AppointmentWindow> AddAsync(AppointmentWindow appointment, CancellationToken cancellationToken);
    Task AddAvailableTimesAsync(List<AppointmentWindow> appointments, CancellationToken cancellationToken);
    Task UpdateAsync(AppointmentWindow appointment, CancellationToken cancellationToken);
    Task DeleteAsync(string appointmentId, CancellationToken cancellationToken);
}
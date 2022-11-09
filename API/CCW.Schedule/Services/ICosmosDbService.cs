using CCW.Schedule.Entities;

namespace CCW.Schedule.Services;

public interface ICosmosDbService
{
    Task<AppointmentWindow> GetAsync(string appointmentId);
    Task<List<AppointmentWindow>> GetAvailableTimesAsync();
    Task<List<AppointmentWindow>> GetAllBookedAppointmentsAsync();
    Task<AppointmentWindow> AddAsync(AppointmentWindow appointment);
    Task AddAvailableTimesAsync(List<AppointmentWindow> appointments);
    Task UpdateAsync(AppointmentWindow appointment);
    Task DeleteAsync(string appointmentId, string userId);
}

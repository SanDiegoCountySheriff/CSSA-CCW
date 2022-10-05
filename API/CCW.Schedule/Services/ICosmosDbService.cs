using CCW.Schedule.Models;

namespace CCW.Schedule.Services;

public interface ICosmosDbService
{
    Task<AppointmentWindow> GetAsync(string appointmentId);
    Task<AppointmentWindow> AddAsync(AppointmentWindow appointment);
    Task UpdateAsync(AppointmentWindow appointment);
    Task DeleteAsync(string appointmentId, string userId);
}

using CCW.Common.Models;

namespace CCW.Application.Services.Contracts;

public interface IAppointmentCosmosDbService
{
    public Task<AppointmentWindow> CreateAppointment(AppointmentWindow appointmentWindow, CancellationToken cancellationToken);
    Task DeleteAppointment(string appointmentId, CancellationToken cancellationToken);
    public Task<int> GetAppointmentLength(CancellationToken cancellationToken);
}

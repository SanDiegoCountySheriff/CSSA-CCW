using CCW.Common.Models;

namespace CCW.Application.Services.Contracts;

public interface IAppointmentCosmosDbService
{
    public Task<AppointmentWindow> CreateAppointment(AppointmentWindow appointmentWindow, CancellationToken cancellationToken);
    public Task<int> GetAppointmentLength(CancellationToken cancellationToken);
}

using CCW.Schedule.Controllers;
using CCW.Schedule.Entities;
using CCW.Schedule.Models;

namespace CCW.Schedule.Clients;

public interface IApplicationServiceClient
{
    Task<HttpResponseMessage> RemoveApplicationAppointmentAsync(string applicationId,
        CancellationToken cancellationToken);

    Task<HttpResponseMessage> UpdateApplicationAppointmentAsync(string applicationId, string appointmentDate,
        CancellationToken cancellationToken);
}
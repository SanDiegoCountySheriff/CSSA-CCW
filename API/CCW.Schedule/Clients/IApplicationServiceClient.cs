namespace CCW.Schedule.Clients;

public interface IApplicationServiceClient
{
    Task<HttpResponseMessage> RemoveApplicationAppointmentAsync(string applicationId,
        CancellationToken cancellationToken);

    Task<HttpResponseMessage> UpdateApplicationAppointmentAsync(string applicationId, string appointmentDate, string appointmentId,
        CancellationToken cancellationToken);

    Task<HttpResponseMessage> AppointmentCheckInByApplicationId(string applicationId, CancellationToken cancellationToken);

    Task<HttpResponseMessage> AppointmentNoShowByApplicationId(string applicationId, CancellationToken cancellationToken);
    Task<HttpResponseMessage> AppointmentScheduledByApplicationId(string applicationId, CancellationToken cancellationToken);
}
namespace CCW.Schedule.Clients;

public class ApplicationServiceClient : IApplicationServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string removeAppointmentUrl;
    private readonly string updateAppointmentUrl;
    private readonly string checkInAppointmentUrl;
    private readonly string noShowAppointmentUrl;
    private readonly string setAppointmentScheduledUrl;

    public ApplicationServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        removeAppointmentUrl = configuration.GetSection("ApplicationServiceClient").GetSection("RemoveAppointmentUrl").Value;
        updateAppointmentUrl = configuration.GetSection("ApplicationServiceClient").GetSection("UpdateAppointmentUrl").Value;
        checkInAppointmentUrl = configuration.GetSection("ApplicationServiceClient").GetSection("CheckInAppointmentUrl").Value;
        noShowAppointmentUrl = configuration.GetSection("ApplicationServiceClient").GetSection("NoShowAppointmentUrl").Value;
        setAppointmentScheduledUrl = configuration.GetSection("ApplicationServiceClient").GetSection("SetAppointmentScheduledUrl").Value;
    }

    public async Task<HttpResponseMessage> RemoveApplicationAppointmentAsync(string applicationId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, removeAppointmentUrl + applicationId);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }


    public async Task<HttpResponseMessage> UpdateApplicationAppointmentAsync(string applicationId, string appointmentDate, string appointmentId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, updateAppointmentUrl +
                                                             "applicationId=" + applicationId +
                                                             "&appointmentDate=" + appointmentDate +
                                                             "&appointmentId=" + appointmentId);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> AppointmentCheckInByApplicationId(string applicationId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{checkInAppointmentUrl}applicationId={applicationId}");

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> AppointmentNoShowByApplicationId(string applicationId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{noShowAppointmentUrl}applicationId={applicationId}");

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> AppointmentScheduledByApplicationId(string applicationId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{setAppointmentScheduledUrl}applicationId={applicationId}");

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }
}
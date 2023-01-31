using CCW.Schedule.Models;

namespace CCW.Schedule.Clients;

public class ApplicationServiceClient: IApplicationServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string removeAppointmentUrl;
    private readonly string updateAppointmentUrl;

    public ApplicationServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        removeAppointmentUrl = configuration.GetSection("ApplicationServiceClient").GetSection("RemoveAppointmentUrl").Value;
        updateAppointmentUrl = configuration.GetSection("ApplicationServiceClient").GetSection("UpdateAppointmentUrl").Value;
    }

    public async Task<HttpResponseMessage> RemoveApplicationAppointmentAsync(string applicationId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, removeAppointmentUrl + applicationId);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }


    public async Task<HttpResponseMessage> UpdateApplicationAppointmentAsync(string applicationId, DateTime appointmentDate, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, updateAppointmentUrl +
                                                             "applicationId=" + applicationId +
                                                             "&appointmentDate=" + appointmentDate);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }
}
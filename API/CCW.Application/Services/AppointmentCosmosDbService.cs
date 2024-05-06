using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using Microsoft.Azure.Cosmos;

namespace CCW.Application.Services;

public class AppointmentCosmosDbService : IAppointmentCosmosDbService
{
    private readonly Container _appointmentContainer;
    private readonly Container _appointmentManagementContainer;

    public AppointmentCosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string appointmentContainerName,
        string appointmentManagementContainerName)
    {
        _appointmentContainer = cosmosDbClient.GetContainer(databaseName, appointmentContainerName);
        _appointmentManagementContainer = cosmosDbClient.GetContainer(databaseName, appointmentManagementContainerName);
    }

    public async Task<AppointmentWindow> CreateAppointment(AppointmentWindow appointmentWindow, CancellationToken cancellationToken)
    {
        return await _appointmentContainer.CreateItemAsync(appointmentWindow, new PartitionKey(appointmentWindow.Id.ToString()), null, cancellationToken);
    }

    public async Task<int> GetAppointmentLength(CancellationToken cancellationToken)
    {
        var appointmentManagement = await _appointmentManagementContainer.ReadItemAsync<AppointmentManagement>("1", new PartitionKey("1"), null, cancellationToken);

        return appointmentManagement.Resource.AppointmentLength;
    }
}

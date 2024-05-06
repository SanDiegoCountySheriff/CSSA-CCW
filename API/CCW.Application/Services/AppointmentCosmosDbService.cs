using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using Microsoft.Azure.Cosmos;

namespace CCW.Application.Services;

public class AppointmentCosmosDbService : IAppointmentCosmosDbService
{
    private readonly Container _container;

    public AppointmentCosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public Task<AppointmentWindow> CreateAppointment(AppointmentWindow appointmentWindow, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

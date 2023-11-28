using CCW.Common.Models;
using Microsoft.Azure.Cosmos;

namespace CCW.Payment.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public async Task<PermitApplication> GetApplication(string applicationId, string userId)
    {
        return await _container.ReadItemAsync<PermitApplication>(applicationId, new PartitionKey(userId));
    }

    public async Task UpdateApplication(PermitApplication application)
    {
        await _container.UpsertItemAsync(application, new PartitionKey(application.UserId));
    }
}

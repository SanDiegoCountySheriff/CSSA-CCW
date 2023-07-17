using CCW.Application.Entities;
using Microsoft.Azure.Cosmos;

namespace CCW.Payment.Services;

public class CosmosDbService : ICosmosDbService
{
    private Container _container;

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public async Task<PermitApplication> GetApplication(string applicationId, string userId, CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a " +
                  "WHERE a.userId = @userId and a.id = @applicationId ";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@userId", userId)
            .WithParameter("@applicationId", applicationId);

        using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery,
            requestOptions: new QueryRequestOptions() { PartitionKey = new PartitionKey(userId) }
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            var application = response.Resource.FirstOrDefault();

            return application;
        }

        return null!;
    }

    public async Task UpdateApplication(PermitApplication application)
    {
        await _container.UpsertItemAsync(application);
    }
}

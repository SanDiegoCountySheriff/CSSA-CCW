using CCW.Common.Models;
using CCW.Schedule.Services.Contracts;
using Microsoft.Azure.Cosmos;

namespace CCW.Schedule.Services;

public class ApplicationCosmosDbService : IApplicationCosmosDbService
{
    private readonly Container _container;

    public ApplicationCosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public async Task<PermitApplication> GetUserApplicationAsync(string applicationId, CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a " +
                   "WHERE a.id = @applicationId ";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@applicationId", applicationId);

        using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            return response.Resource.FirstOrDefault();
        }

        return null!;
    }

    public async Task UpdateUserApplicationAsync(PermitApplication application, CancellationToken cancellationToken)
    {
        await _container.UpsertItemAsync(application, new PartitionKey(application.UserId));
    }
}

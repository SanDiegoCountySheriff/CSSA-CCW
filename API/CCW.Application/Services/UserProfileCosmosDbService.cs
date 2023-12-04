using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using Microsoft.Azure.Cosmos;

namespace CCW.Application.Services;

public class UserProfileCosmosDbService : IUserProfileCosmosDbService
{
    private readonly Container _container;

    public UserProfileCosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public async Task<AdminUser> GetAdminUserProfileAsync(string licensingUserName, CancellationToken cancellationToken)
    {
        var queryString = "SELECT * FROM c WHERE c.id = @adminUserId";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@adminUserId", licensingUserName);

        using FeedIterator<AdminUser> filteredFeed = _container.GetItemQueryIterator<AdminUser>(
            queryDefinition: parameterizedQuery,
            requestOptions: new QueryRequestOptions() { PartitionKey = new PartitionKey(licensingUserName) }
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<AdminUser> response = await filteredFeed.ReadNextAsync(cancellationToken);

            return response.Resource.FirstOrDefault();
        }

        return null!;
    }
}
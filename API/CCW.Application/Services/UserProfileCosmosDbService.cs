using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using Microsoft.Azure.Cosmos;

namespace CCW.Application.Services;

public class UserProfileCosmosDbService : IUserProfileCosmosDbService
{
    private readonly Container _container;
    private readonly Container _userContainer;

    public UserProfileCosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName,
        string userContainerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
        _userContainer = cosmosDbClient.GetContainer(databaseName, userContainerName);
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

    public async Task<Common.Models.User> GetUser(string id, CancellationToken cancellationToken)
    {
        return await _userContainer.ReadItemAsync<Common.Models.User>(id, new PartitionKey(id), null, cancellationToken);
    }

    public async Task UpdateUser(Common.Models.User user, CancellationToken cancellationToken)
    {
        await _userContainer.UpsertItemAsync(user, new PartitionKey(user.Id), null, cancellationToken);
    }
}

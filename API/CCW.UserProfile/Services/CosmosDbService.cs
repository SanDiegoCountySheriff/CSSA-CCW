using CCW.Common.Models;
using Microsoft.Azure.Cosmos;
using System.Net;
using User = CCW.Common.Models.User;

namespace CCW.UserProfile.Services;

public class CosmosDbService : ICosmosDbService
{
    private Container _adminUserContainer;
    private Container _userContainer;

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string adminUsersContainerName, string usersContainerName)
    {
        _adminUserContainer = cosmosDbClient.GetContainer(databaseName, adminUsersContainerName);
        _userContainer = cosmosDbClient.GetContainer(databaseName, usersContainerName);
    }

    public async Task<AdminUser> AddAdminUserAsync(AdminUser adminUser, CancellationToken cancellationToken)
    {
        return await _adminUserContainer.UpsertItemAsync(adminUser, new PartitionKey(adminUser.Id), null, cancellationToken);
    }

    public async Task<AdminUser> GetAdminUserAsync(string adminUserId, CancellationToken cancellationToken)
    {
        try
        {
            var queryString = "SELECT * FROM users p WHERE p.id = @adminUserId";

            var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@adminUserId", adminUserId);

            using FeedIterator<AdminUser> filteredFeed = _adminUserContainer.GetItemQueryIterator<AdminUser>(
                queryDefinition: parameterizedQuery,
                requestOptions: new QueryRequestOptions() { PartitionKey = new PartitionKey(adminUserId) }
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<AdminUser> response = await filteredFeed.ReadNextAsync(cancellationToken);

                return response.Resource.FirstOrDefault();
            }

            return null!;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<AdminUser>> GetAllAdminUsers(CancellationToken cancellationToken)
    {
        List<AdminUser> adminUsers = new List<AdminUser>();

        var parameterizedQuery = new QueryDefinition("SELECT * FROM c");

        using FeedIterator<AdminUser> feedIterator = _adminUserContainer.GetItemQueryIterator<AdminUser>(
            queryDefinition: parameterizedQuery
        );

        while (feedIterator.HasMoreResults)
        {
            foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
            {
                adminUsers.Add(item);
            }
        }

        return adminUsers;
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        return await _adminUserContainer.UpsertItemAsync(user, new PartitionKey(user.Id), null, cancellationToken);
    }

    public async Task<AdminUser> GetUserAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            var queryString = "SELECT * FROM users p WHERE p.id = @adminUserId";

            var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@userId", userId);

            using FeedIterator<User> filteredFeed = _userContainer.GetItemQueryIterator<User>(
                queryDefinition: parameterizedQuery,
                requestOptions: new QueryRequestOptions() { PartitionKey = new PartitionKey(userId) }
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<AdminUser> response = await filteredFeed.ReadNextAsync(cancellationToken);

                return response.Resource.FirstOrDefault();
            }

            return null!;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null!;
        }
    }
}

using CCW.Common.Models;
using CCW.Common.Services;
using Microsoft.Azure.Cosmos;
using System.Net;
using User = CCW.Common.Models.User;

namespace CCW.UserProfile.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDatabaseContainerResolver _databaseContainerResolver;
    private readonly Container _adminUserContainer;
    private readonly Container _userContainer;

    public CosmosDbService(
        IHttpContextAccessor contextAccessor,
        IDatabaseContainerResolver databaseContainerResolver
    )
    {
        _contextAccessor = contextAccessor;
        _databaseContainerResolver = databaseContainerResolver;

        var tenantId = _contextAccessor.HttpContext.Items["TenantId"].ToString();

        _adminUserContainer = _databaseContainerResolver.GetContainer(tenantId, "admin-users");
        _userContainer = _databaseContainerResolver.GetContainer(tenantId, "users");
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
        return await _userContainer.CreateItemAsync(user, new PartitionKey(user.Id), null, cancellationToken);
    }

    public async Task<User> UpdateUserAsync(User user, string userId, CancellationToken cancellationToken)
    {
        user.Id = userId;
        return await _userContainer.UpsertItemAsync(user, new PartitionKey(userId), null, cancellationToken);
    }

    public async Task<User> GetUserAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            var queryString = "SELECT * FROM users p WHERE p.id = @userId";

            var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@userId", userId);

            using FeedIterator<User> filteredFeed = _userContainer.GetItemQueryIterator<User>(
                queryDefinition: parameterizedQuery,
                requestOptions: new QueryRequestOptions() { PartitionKey = new PartitionKey(userId) }
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<User> response = await filteredFeed.ReadNextAsync(cancellationToken);

                return response.Resource.FirstOrDefault();
            }

            return null!;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<User>> GetAllUsers(CancellationToken cancellationToken)
    {
        List<User> users = new List<User>();

        var parameterizedQuery = new QueryDefinition("SELECT * FROM c");

        using FeedIterator<User> feedIterator = _userContainer.GetItemQueryIterator<User>(
            queryDefinition: parameterizedQuery
        );

        while (feedIterator.HasMoreResults)
        {
            foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
            {
                users.Add(item);
            }
        }

        return users;
    }

    public async Task<List<User>> GetUnmatchedUserProfiles(CancellationToken cancellationToken)
    {
        List<User> users = new List<User>();

        var queryString = new QueryDefinition("SELECT * FROM c WHERE c.isPendingReview = true");

        using FeedIterator<User> feedIterator = _userContainer.GetItemQueryIterator<User>(
            queryDefinition: queryString
        );

        while (feedIterator.HasMoreResults)
        {
            foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
            {
                users.Add(item);
            }
        }

        return users;
    }
}

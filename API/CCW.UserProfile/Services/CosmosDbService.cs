using CCW.UserProfile.Entities;
using Microsoft.Azure.Cosmos;
using System.Net;
using Container = Microsoft.Azure.Cosmos.Container;
using User = CCW.UserProfile.Entities.User;

namespace CCW.UserProfile.Services;

public class CosmosDbService : ICosmosDbService
{
    private Container _container;
    private Container _adminUserContainer;

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName,
        string adminUsersContainerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
        _adminUserContainer = cosmosDbClient.GetContainer(databaseName, adminUsersContainerName);
    }

    public async Task<AdminUser?> AddAdminUserAsync(AdminUser adminUser, CancellationToken cancellationToken)
    {
        return await _adminUserContainer.UpsertItemAsync(adminUser, new PartitionKey(adminUser.Id), null, cancellationToken);
    }

    public async Task<AdminUser?> GetAdminUserAsync(string adminUserId, CancellationToken cancellationToken)
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

    public async Task<User?> AddAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = await GetAsync(user.Id, default);

        if (existingUser != null)
        {
            if (existingUser.UserEmail == user.UserEmail)
            {
                throw new ArgumentException("Email address already exists.");
            }

            foreach (var email in existingUser.PreviousEmails)
            {
                if (email.EmailAddress == user.UserEmail)
                {
                    throw new ArgumentException("Email address used in past.");
                }
            }

            var previousEmails = existingUser.PreviousEmails.ToList();

            previousEmails.Add(new Email
            {
                EmailAddress = existingUser.UserEmail,
                CreateDateTimeUtc = existingUser.ProfileUpdateDateTimeUtc,
            });

            user.PreviousEmails = previousEmails.ToArray();

            await _container.PatchItemAsync<User>(
                user.Id,
                new PartitionKey(user.Id),
                new[]
                {
                    PatchOperation.Set("/userEmail", user.UserEmail),
                    PatchOperation.Set("/previousEmails", user.PreviousEmails),
                    PatchOperation.Set("/profileUpdateDateTimeUtc", DateTime.UtcNow),
                },
                null,
                cancellationToken
            );

            return user;
        }

        user.PreviousEmails = Array.Empty<Email>();
        user.UserCreateDateTimeUtc = DateTime.UtcNow;
        user.ProfileUpdateDateTimeUtc = DateTime.UtcNow;

        User createdItem = await _container.CreateItemAsync(user, new PartitionKey(user.Id), null, cancellationToken);
        return createdItem;
    }

    public async Task<User?> GetAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            var queryString = "SELECT * FROM users p WHERE p.id = @userId";

            var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@userId", userId);

            using FeedIterator<User> filteredFeed = _container.GetItemQueryIterator<User>(
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
}

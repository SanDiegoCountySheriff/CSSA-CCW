using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using CCW.Common.Services;
using Microsoft.Azure.Cosmos;

namespace CCW.Application.Services;

public class UserProfileCosmosDbService : IUserProfileCosmosDbService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDatabaseContainerResolver _databaseContainerResolver;
    private readonly Container _adminUserContainer;
    private readonly Container _userContainer;

    public UserProfileCosmosDbService(
        IHttpContextAccessor contextAccessor,
        IDatabaseContainerResolver databaseContainerResolver,
        IConfiguration configuration
    )
    {
        _contextAccessor = contextAccessor;
        _databaseContainerResolver = databaseContainerResolver;

        var tenantId = _contextAccessor.HttpContext.Items["TenantId"] != null ? _contextAccessor.HttpContext.Items["TenantId"].ToString() : "";
        var adminUserContainerName = configuration.GetSection("CosmosDb").GetSection("UserProfileContainerName").Value;
        var userContainerName = configuration.GetSection("CosmosDb").GetSection("UserContainerName").Value;

        _adminUserContainer = _databaseContainerResolver.GetContainer(tenantId, adminUserContainerName);
        _userContainer = _databaseContainerResolver.GetContainer(tenantId, userContainerName);
    }

    public async Task<AdminUser> GetAdminUserProfileAsync(string licensingUserName, CancellationToken cancellationToken)
    {
        var queryString = "SELECT * FROM c WHERE c.id = @adminUserId";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@adminUserId", licensingUserName);

        using FeedIterator<AdminUser> filteredFeed = _adminUserContainer.GetItemQueryIterator<AdminUser>(
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

using CCW.Application.Models;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;

namespace CCW.Application.Services;

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


    public async Task<PermitApplication> AddAsync(PermitApplication application)
    {
        try
        {
            PermitApplication createdItem = await _container.CreateItemAsync(application, new PartitionKey(application.UserId));
            return createdItem;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task DeleteAsync(string applicationId, string userId)
    {
        await _container.DeleteItemAsync<User>(applicationId, new PartitionKey(userId));
    }

    public async Task<PermitApplication> GetAsync(string applicationId)
    {
        try
        {
            // Build query definition
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM users p WHERE p.id = @applicationId"
            )
                .WithParameter("@applicationId", applicationId);


            // Query multiple items from container
            using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
                queryDefinition: parameterizedQuery
            );

            // Iterate query result pages
            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync();

                var user = response.Select(u => new PermitApplication
                {
                    Id = u.Id,
                    Address = u.Address,
                    City = u.City,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PermitType = u.PermitType,
                    State = u.State,
                    UserId = u.UserId,
                    ZipCode = u.ZipCode
                }).First();

                return user;
            }

            return null!;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<PermitApplication>> GetMultipleAsync(string queryString)
    {
        var query = _container.GetItemQueryIterator<PermitApplication>(new QueryDefinition(queryString));
        var results = new List<PermitApplication>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }
        return results;
    }

    public async Task UpdateAsync(PermitApplication application)
    {
        await _container.UpsertItemAsync(application, new PartitionKey(application.UserId));
    }
}

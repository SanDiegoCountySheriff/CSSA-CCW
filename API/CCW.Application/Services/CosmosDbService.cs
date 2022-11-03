using CCW.Application.Entities;
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
            PermitApplication createdItem = await _container.CreateItemAsync(application, new PartitionKey(application.Id.ToString()));
            return createdItem;
        }
        catch (CosmosException ex)
        {
            throw;
        }
    }

    public async Task DeleteAsync(string applicationId, string userId)
    {
        await _container.DeleteItemAsync<User>(applicationId, new PartitionKey(userId));
    }

    public async Task<PermitApplication> GetAsync(string userEmail, bool isComplete)
    {
        try
        {
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT a.Application, a.id FROM applications a join a.Application ap join ap.UserEmail as e where e = @userEmail and ap.IsComplete = @isComplete"
                )
                .WithParameter("@userEmail", userEmail)
                .WithParameter("@isComplete", isComplete);

            using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
                queryDefinition: parameterizedQuery
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync();

                var application = response.Resource.FirstOrDefault(); 
 
                return application;
            }

            return null!;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<PermitApplication>> ListAsync(int startIndex, int count)
    {
        try
        {
            // Build query definition
            var parameterizedQuery = new QueryDefinition(query: "SELECT * FROM users p OFFSET @offSet LIMIT @limit")
                .WithParameter("@offSet", startIndex)
                .WithParameter("@limit", count);

            // Query multiple items from container
            using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(parameterizedQuery);

            List<PermitApplication> applications = new List<PermitApplication>(count);

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                var feedResult = await filteredFeed.ReadNextAsync();

                return feedResult.ToList();
            }

            return new List<PermitApplication>(0);
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<PermitApplication>> GetMultipleAsync()
    {
        var query = _container.GetItemQueryIterator<PermitApplication>();
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
        await _container.UpsertItemAsync(application, new PartitionKey(application.Id.ToString()));
    }
}

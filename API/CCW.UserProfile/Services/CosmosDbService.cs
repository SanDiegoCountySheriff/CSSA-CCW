
using CCW.UserProfile.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using System.ComponentModel;
using System.Net;
using System.Threading;
using Container = Microsoft.Azure.Cosmos.Container;
using User = CCW.UserProfile.Models.User;

namespace CCW.UserProfile.Services;

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

    public async Task<User> AddAsync(User user)
    {
        User createdItem =   await _container.CreateItemAsync(user, new PartitionKey(user.Id));
        return createdItem;
    }

    public async Task<User?> GetAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            // Build query definition
            var query = "SELECT * FROM users p WHERE p.email = @email";
            (string paramName, object paramValue)[] parameters = {
                ("@email", email)
            };

            using var feedIterator = CreateFeedIterator<User>(_container, query, parameters);

            if (feedIterator.HasMoreResults)
            {
                var response = await feedIterator.ReadNextAsync(cancellationToken);

                var results = response.Resource.ToArray();

                if (results.Length == 1)
                {
                    return results[0];
                }
            }

            return null;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async Task DeleteAsync(string id)
    {
        await _container.DeleteItemAsync<User>(id, new PartitionKey(id));
    }

    public async Task<IEnumerable<User>> GetMultipleAsync(string queryString)
    {
        var query = _container.GetItemQueryIterator<User>(new QueryDefinition(queryString));
        var results = new List<User>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }
        return results;
    }

    public async Task UpdateAsync(string id, User user)
    {
        await _container.UpsertItemAsync(user, new PartitionKey(id));
    }

    private static FeedIterator<T> CreateFeedIterator<T>(Container container, string query,
        (string paramName, object paramValue)[] parameters)
    {
        var queryDefinition = new QueryDefinition(query);

        foreach (var (parameterName, parameterValue) in parameters)
        {
            queryDefinition = queryDefinition.WithParameter(parameterName, parameterValue);
        }

        using var feedIterator = container.GetItemQueryIterator<T>(queryDefinition);
        return feedIterator;
    }
}


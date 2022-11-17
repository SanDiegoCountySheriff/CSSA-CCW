using Microsoft.Azure.Cosmos;
using System.Net;
using Container = Microsoft.Azure.Cosmos.Container;
using User = CCW.UserProfile.Entities.User;

namespace CCW.UserProfile.Services;

public class CosmosDbService : ICosmosDbService
{
    private Container _container;
    private readonly ILogger<CosmosDbService> _logger;

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName,
        ILogger<CosmosDbService> logger)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
        _logger = logger;
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            User createdItem = await _container.CreateItemAsync(user, new PartitionKey(user.Id), null, cancellationToken);
            return createdItem;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to add a user: {cex.Message}");
            throw new Exception("An error occur while trying to add a user.");
        }
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
        catch (CosmosException cex) when (cex.StatusCode != HttpStatusCode.NotFound)
        {
            _logger.LogWarning($"An error occur while trying to get user: {cex.Message}");
            throw new Exception("An error occur while trying to get user.");
        }
    }

    //public async Task<User?> GetUserAsync(string id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        // Build query definition
    //        var query = "SELECT * FROM users p WHERE p.id = @id";
    //        (string paramName, object paramValue)[] parameters = {
    //            ("@id", id)
    //        };

    //        using var feedIterator = CreateFeedIterator<User>(_container, query, parameters);

    //        if (feedIterator.HasMoreResults)
    //        {
    //            var response = await feedIterator.ReadNextAsync(cancellationToken);

    //            var results = response.Resource.ToArray();

    //            if (results.Length == 1)
    //            {
    //                return results[0];
    //            }
    //        }

    //        return null;
    //    }
    //    catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    //    {
    //        return default;
    //    }
    //}

    //public async Task DeleteAsync(string id)
    //{
    //    await _container.DeleteItemAsync<User>(id, new PartitionKey(id));
    //}

    //public async Task<IEnumerable<User>> GetMultipleAsync(string queryString)
    //{
    //    var query = _container.GetItemQueryIterator<User>(new QueryDefinition(queryString));
    //    var results = new List<User>();
    //    while (query.HasMoreResults)
    //    {
    //        var response = await query.ReadNextAsync();
    //        results.AddRange(response.ToList());
    //    }
    //    return results;
    //}

    //public async Task UpdateAsync(string id, User user)
    //{
    //    await _container.UpsertItemAsync(user, new PartitionKey(id));
    //}

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


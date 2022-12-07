using Microsoft.Azure.Cosmos;
using System.Net;
using Container = Microsoft.Azure.Cosmos.Container;
using User = CCW.UserProfile.Entities.User;


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

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        User createdItem = await _container.CreateItemAsync(user, new PartitionKey(user.Id), null, cancellationToken);
        return createdItem;
    }

    public async Task<User?> GetAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
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
            return null!;
        }
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

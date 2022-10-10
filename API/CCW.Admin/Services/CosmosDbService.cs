using CCW.Admin.Models;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace CCW.Admin.Services;

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

    public async Task<AgencyProfileSettings> GetSettingsAsync(string agencyId, CancellationToken cancellationToken)
    {
        try
        {
            var query = "SELECT * FROM users p WHERE p.Id = @agencyId";
            (string paramName, object paramValue)[] parameters = {
                ("@agencyId", agencyId)
            };

            using var feedIterator = CreateFeedIterator<AgencyProfileSettings>(_container, query, parameters);

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

    public async Task<AgencyProfileSettings> AddAsync(AgencyProfileSettings agencyProfile, CancellationToken cancellationToken)
    {
        AgencyProfileSettings createdItem = await _container.CreateItemAsync(agencyProfile, new PartitionKey(agencyProfile.Id));
        return createdItem;
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

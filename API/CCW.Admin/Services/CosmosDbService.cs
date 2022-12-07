using Microsoft.Azure.Cosmos;
using System.Net;
using CCW.Admin.Entities;


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

    public async Task<AgencyProfileSettings?> GetSettingsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var query = "SELECT * FROM agency";
            using var feedIterator = CreateFeedIterator<AgencyProfileSettings>(_container, query);

            if (feedIterator.HasMoreResults)
            {
                var response = await feedIterator.ReadNextAsync(cancellationToken);

                var results = response.Resource.ToArray();

                if (results.Length > 0)
                {
                    return results[0];
                }
            }

            return null!;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null!;
        }
    }

    public async Task<AgencyProfileSettings> UpdateSettingsAsync(AgencyProfileSettings agencyProfile, CancellationToken cancellationToken)
    {
        var storedProfile = await GetSettingsAsync(cancellationToken);
        if (storedProfile?.AgencyName == null)
        {
            agencyProfile.Id = Guid.NewGuid().ToString();
            AgencyProfileSettings createdItem = await _container.CreateItemAsync(agencyProfile, new PartitionKey(agencyProfile.Id), null, cancellationToken);

            return createdItem;
        }

        agencyProfile.Id = storedProfile.Id;
        var result = await _container.UpsertItemAsync(agencyProfile, new PartitionKey(agencyProfile.Id), null, cancellationToken);

        return result.Resource;
    }

    private static FeedIterator<T> CreateFeedIterator<T>(Container container, string query, params (string paramName, object paramValue)[] parameters)
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

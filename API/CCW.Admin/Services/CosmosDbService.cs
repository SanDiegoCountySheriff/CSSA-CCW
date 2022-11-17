using Microsoft.Azure.Cosmos;
using System.Net;
using CCW.Admin.Entities;
using System.Runtime.ConstrainedExecution;

namespace CCW.Admin.Services;

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

    public async Task<AgencyProfileSettings> GetSettingsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var query = "SELECT * FROM agencies";
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
            return default!;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve agency settings: {cex.Message}");
            throw new Exception("An error occur while trying to retrieve agency settings.");
        }
    }

    public async Task<AgencyProfileSettings> AddSettingsAsync(AgencyProfileSettings agencyProfile, CancellationToken cancellationToken)
    {
        try
        {
            AgencyProfileSettings createdItem = await _container.CreateItemAsync(agencyProfile, new PartitionKey(agencyProfile.Id), null, cancellationToken);

            return createdItem;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to add agency settings: {cex.Message}");
            throw new Exception("An error occur while trying to add agency settings.");
        }
    }

    public async Task<AgencyProfileSettings> UpdateSettingsAsync(AgencyProfileSettings agencyProfile, CancellationToken cancellationToken)
    {
        try
        {
            var storedProfile = await GetSettingsAsync(cancellationToken);
            if (storedProfile?.AgencyName == null)
            {
                AgencyProfileSettings createdItem = await _container.CreateItemAsync(agencyProfile, new PartitionKey(agencyProfile.Id), null, cancellationToken);

                return createdItem;
            }

            agencyProfile.Id = storedProfile.Id;
            var result = await _container.UpsertItemAsync(agencyProfile, new PartitionKey(agencyProfile.Id), null, cancellationToken);

            return result.Resource;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to update or create agency settings: {cex.Message}");
            throw new Exception("An error occur while trying to update or create agency settings.");
        }
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

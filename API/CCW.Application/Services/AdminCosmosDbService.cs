using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using Microsoft.Azure.Cosmos;

namespace CCW.Application.Services;

public class AdminCosmosDbService : IAdminCosmosDbService
{
    private readonly Container _container;

    public AdminCosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public async Task<AgencyProfileSettings> GetAgencyProfileSettingsAsync(CancellationToken cancellationToken)
    {
        var query = "SELECT * FROM agency";

        var queryDefinition = new QueryDefinition(query);

        using var feedIterator = _container.GetItemQueryIterator<AgencyProfileSettings>(queryDefinition);

        if (feedIterator.HasMoreResults)
        {
            var response = await feedIterator.ReadNextAsync(cancellationToken);

            return response.Resource.FirstOrDefault();
        }

        return null!;
    }
}

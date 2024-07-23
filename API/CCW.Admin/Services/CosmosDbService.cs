using CCW.Common.Models;
using CCW.Common.Services;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace CCW.Admin.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDatabaseContainerResolver _databaseContainerResolver;
    private readonly string _tenantId;
    private readonly Container _container;

    public CosmosDbService(
        IHttpContextAccessor contextAccessor,
        IDatabaseContainerResolver databaseContainerResolver,
        IConfiguration configuration
    )
    {
        _contextAccessor = contextAccessor;
        _databaseContainerResolver = databaseContainerResolver;

        _tenantId = _contextAccessor.HttpContext.Items["TenantId"] != null ? _contextAccessor.HttpContext.Items["TenantId"].ToString() : "";
        var agencyContainerName = configuration.GetSection("CosmosDb").GetSection("ContainerName").Value;

        if (!string.IsNullOrWhiteSpace(_tenantId))
        {
            _container = _databaseContainerResolver.GetContainer(_tenantId, agencyContainerName);
        }
    }

    public async Task<AgencyProfileSettings> GetSettingsAsync(string tenantId, CancellationToken cancellationToken)
    {
        try
        {
            var container = _databaseContainerResolver.GetContainer(tenantId, "agency");

            var query = "SELECT * FROM agency";
            using var feedIterator = CreateFeedIterator<AgencyProfileSettings>(container, query);

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
        var storedProfile = await GetSettingsAsync(_tenantId, cancellationToken);

        if (storedProfile?.AgencyName == null)
        {
            agencyProfile.Id = Guid.NewGuid().ToString();
        }
        else
        {
            agencyProfile.Id = storedProfile.Id;
        }

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

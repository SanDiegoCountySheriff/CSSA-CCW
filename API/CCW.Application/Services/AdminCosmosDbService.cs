using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using CCW.Common.Services;
using Microsoft.Azure.Cosmos;

namespace CCW.Application.Services;

public class AdminCosmosDbService : IAdminCosmosDbService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDatabaseContainerResolver _databaseContainerResolver;
    private readonly Container _container;

    public AdminCosmosDbService(
        IHttpContextAccessor contextAccessor,
        IDatabaseContainerResolver databaseContainerResolver)
    {
        _contextAccessor = contextAccessor;
        _databaseContainerResolver = databaseContainerResolver;

        var tenantId = _contextAccessor.HttpContext.Items["TenantId"] != null ? _contextAccessor.HttpContext.Items["TenantId"].ToString() : "";

        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            _container = _databaseContainerResolver.GetContainer(tenantId, "agency");
        }
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

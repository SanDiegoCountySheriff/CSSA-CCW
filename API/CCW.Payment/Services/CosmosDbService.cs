using CCW.Common.Models;
using Microsoft.Azure.Cosmos;
using System;

namespace CCW.Payment.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;
    private readonly Container _refundRequestContainer;

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName,
        string refundRequestContainerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
        _refundRequestContainer = cosmosDbClient.GetContainer(databaseName, refundRequestContainerName);
    }

    public async Task<PermitApplication> GetAdminApplication(string applicationId)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a WHERE a.id = @applicationId ";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@applicationId", applicationId);

        using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync();

            return response.Resource.FirstOrDefault();
        }

        return null!;
    }

    public async Task<PermitApplication> GetApplication(string applicationId, string userId)
    {
        return await _container.ReadItemAsync<PermitApplication>(applicationId, new PartitionKey(userId));
    }

    public async Task AddRefundRequest(RefundRequest refundRequest)
    {
        refundRequest.Id = Guid.NewGuid().ToString();
        await _refundRequestContainer.CreateItemAsync(refundRequest, new PartitionKey(refundRequest.Id));
    }

    public async Task UpdateApplication(PermitApplication application)
    {
        await _container.UpsertItemAsync(application, new PartitionKey(application.UserId));
    }

    public async Task<IEnumerable<RefundRequest>> GetAllRefundRequests()
    {
        var queryString = "SELECT * FROM c";

        var query = new QueryDefinition(query: queryString);

        using FeedIterator<RefundRequest> filteredFeed = _refundRequestContainer.GetItemQueryIterator<RefundRequest>(query);

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<RefundRequest> response = await filteredFeed.ReadNextAsync();

            return response.Resource;
        }

        return null!;
    }
}

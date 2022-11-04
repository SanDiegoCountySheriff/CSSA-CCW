using CCW.Application.Entities;
using CCW.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using System.Linq;
using Container = Microsoft.Azure.Cosmos.Container;

namespace CCW.Application.Services;

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

    public async Task<PermitApplication> AddAsync(PermitApplication application)
    {
        try
        {
            PermitApplication createdItem = await _container.CreateItemAsync(application, new PartitionKey(application.Id.ToString()));
            return createdItem;
        }
        catch (CosmosException ex)
        {
            throw;
        }
    }

    public async Task DeleteAsync(string applicationId, string userId)
    {
        await _container.DeleteItemAsync<User>(applicationId, new PartitionKey(userId));
    }

    public async Task<PermitApplication> GetAsync(string userEmailOrOrderId, bool isOrderId, bool isComplete)
    {
        try
        {
            var queryString = isOrderId
                ? "SELECT a.Application, a.id FROM applications " +
                  "a join a.Application ap join ap.OrderId as e " +
                  "where e = @userEmailOrOrderId and ap.IsComplete = @isComplete"
                : "SELECT a.Application, a.id FROM applications a " +
                  "join a.Application ap join ap.UserEmail as e " +
                  "where e = @userEmailOrOrderId and ap.IsComplete = @isComplete";

            var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@userEmailOrOrderId", userEmailOrOrderId)
                .WithParameter("@isComplete", isComplete);

            using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
                queryDefinition: parameterizedQuery
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync();

                var application = response.Resource.FirstOrDefault();

                return application;
            }

            return null!;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<PermitApplication>> ListAsync(int startIndex, int count)
    {
        try
        {
            // Build query definition
            var parameterizedQuery = new QueryDefinition(query: "SELECT * FROM users p OFFSET @offSet LIMIT @limit")
                .WithParameter("@offSet", startIndex)
                .WithParameter("@limit", count);

            // Query multiple items from container
            using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(parameterizedQuery);

            List<PermitApplication> applications = new List<PermitApplication>(count);

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                var feedResult = await filteredFeed.ReadNextAsync();

                return feedResult.ToList();
            }

            return new List<PermitApplication>(0);
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<PermitApplication>> GetMultipleAsync()
    {
        var query = _container.GetItemQueryIterator<PermitApplication>();
        var results = new List<PermitApplication>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task<IEnumerable<SummarizedPermitApplication>> GetAllApplicationsAsync()
    {
        var query = new QueryDefinition(
            query:
            "SELECT a.Application.UserEmail, a.Application.CurrentAddress, a.Application.PersonalInfo.LastName, a.Application.PersonalInfo.FirstName," +
            "a.Application.ApplicationStatus, a.Application.AppointmentStatus, a.Application.OrderId, a.id" +
            "FROM applications a join a.Application where Application.OrderId ='123ABC'"
        );

        //using FeedIterator<SummarizedPermitApplication> resultSet = _container.GetItemQueryIterator<SummarizedPermitApplication>(
        //    queryDefinition: query,
        //    requestOptions: new QueryRequestOptions
        //    {
        //        MaxConcurrency = 1
        //    }
        //);

        var results = new List<SummarizedPermitApplication>();
        //if (resultSet.HasMoreResults)
        //{
        //    var response = await resultSet.ReadNextAsync();

        //    return response.Resource;
        //}

        using (var appsIterator = _container.GetItemQueryIterator<SummarizedPermitApplication>(query))
        {
            while (appsIterator.HasMoreResults)
            {
                var apps = await appsIterator.ReadNextAsync();
                foreach (var item in apps)
                {
                    results.Add(item);
                }
            }
        }

        return results;
    }

    public async Task UpdateAsync(PermitApplication application)
    {
        await _container.UpsertItemAsync(application, new PartitionKey(application.Id.ToString()));
    }
}

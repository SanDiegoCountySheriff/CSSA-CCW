using CCW.Application.Entities;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
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
            "SELECT " +
            "a.Application.UserEmail as UserEmail, " +
            "a.Application.CurrentAddress as CurrentAddress, " +
            "a.Application.PersonalInfo.LastName as LastName, " +
            "a.Application.PersonalInfo.FirstName as FirstName, " +
            "a.Application.ApplicationStatus as ApplicationStatus, " +
            "a.Application.AppointmentStatus as AppointmentStatus, " +
            "a.Application.OrderId as OrderId, " +
            "a.id " +
            "FROM a"
        );

        var results = new List<SummarizedPermitApplication>();
        using (var appsIterator = _container.GetItemQueryIterator<SummarizedPermitApplication>(query))
        {
            while (appsIterator.HasMoreResults)
            {
                FeedResponse<SummarizedPermitApplication> apps = await appsIterator.ReadNextAsync();
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
        var modelS = JsonConvert.SerializeObject(application.History[0]);
        var model = JsonConvert.DeserializeObject<History>(modelS);
        var history = new History
        {
            ChangeMadeBy = model.ChangeMadeBy,
            Change = model.Change,
            ChangeDateTimeUtc = model.ChangeDateTimeUtc,
        };

        await _container.PatchItemAsync<PermitApplication>(
            application.Id.ToString(),
            new PartitionKey(application.Id.ToString()),
            new[]
            {
                PatchOperation.Set("/Application", application.Application),
                PatchOperation.Add("/History/-", history)
            }
            );
    }

    public async Task<IEnumerable<History>> GetApplicationHistoryAsync(string applicationIdOrOrderId, bool isOrderId = false)
    {
        try
        {
            var result = new List<History>();
            var queryString = isOrderId
                ? "SELECT a.History FROM applications " +
                  "a join a.Application ap join ap.OrderId as e " +
                  "where ap.OrderId = @applicationIdOrOrderId"
                : "SELECT a.History FROM a " +
                  "where a.id = @applicationIdOrOrderId";

            var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@applicationIdOrOrderId", applicationIdOrOrderId);

            using FeedIterator<HistoryResponse> filteredFeed = _container.GetItemQueryIterator<HistoryResponse>(
                queryDefinition: parameterizedQuery
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<HistoryResponse> response = await filteredFeed.ReadNextAsync();

                var history = response.Resource.ToList();

                for (int i = 0; i < history[0].History.Length; i++)
                {
                    result.Add(history[0].History[i]);
                }

                return result;
            }

            return new List<History>();
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
}

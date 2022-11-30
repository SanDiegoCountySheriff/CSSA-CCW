using CCW.Application.Entities;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using Container = Microsoft.Azure.Cosmos.Container;

namespace CCW.Application.Services;

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

    public async Task<PermitApplication> AddAsync(PermitApplication application, CancellationToken cancellationToken)
    {
        try
        {
            PermitApplication createdItem = await _container.CreateItemAsync(application, new PartitionKey(application.Id.ToString()), null, cancellationToken);
            return createdItem;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to add new permit application: {cex.Message}");
            throw new Exception("An error occur while trying to add new permit application.");
        }
    }

    public async Task DeleteAsync(string applicationId, string userId, CancellationToken cancellationToken)
    {
        try
        {
            await _container.DeleteItemAsync<PermitApplication>(applicationId, new PartitionKey(applicationId), null, cancellationToken);
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to delete permit application: {cex.Message}");
            throw new Exception("An error occur while trying to delete permit application.");
        }
    }

    public async Task<PermitApplication> GetLastApplicationAsync(string userEmailOrOrderId, bool isOrderId, bool isComplete, CancellationToken cancellationToken)
    {
        try
        {
            var queryString = isOrderId
                ? "SELECT a.Application, a.id, a.History FROM applications " +
                  "a join a.Application ap join ap.OrderId as e " +
                  "where e = @userEmailOrOrderId and ap.IsComplete = @isComplete Order by a.OrderId DESC"
                : "SELECT a.Application, a.id, a.History FROM applications a " +
                  "join a.Application ap join ap.UserEmail as e " +
                  "where e = @userEmailOrOrderId and ap.IsComplete = @isComplete Order by a.OrderId DESC";

            var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@userEmailOrOrderId", userEmailOrOrderId)
                .WithParameter("@isComplete", isComplete);

            using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
                queryDefinition: parameterizedQuery
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

                var application = response.Resource.FirstOrDefault();

                return application;
            }

            return null!;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve permit application: {cex.Message}");
            throw new Exception("An error occur while trying to retrieve permit application.");
        }
    }

    public async Task<IEnumerable<PermitApplication>> GetAllUserApplicationsAsync(string userEmail, CancellationToken cancellationToken)
    {
        try
        {
            var queryString = "SELECT a.Application, a.id FROM applications a " +
                  "join a.Application ap join ap.UserEmail as e " +
                  "where e = @userEmail Order by a.OrderId DESC";

            var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@userEmail", userEmail);
                 

            using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
                queryDefinition: parameterizedQuery
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

                return response.Resource;
            }

            return null!;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve permit application: {cex.Message}");
            throw new Exception("An error occur while trying to retrieve permit application.");
        }
    }

    public async Task<IEnumerable<PermitApplication>> ListAsync(int startIndex, int count, CancellationToken cancellationToken)
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
                var feedResult = await filteredFeed.ReadNextAsync(cancellationToken);

                return feedResult.ToList();
            }

            return new List<PermitApplication>(0);
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve all permit applications: {cex.Message}");
            throw new Exception("An error occur while trying to retrieve all permit applications.");
        }
    }

    public async Task<IEnumerable<PermitApplication>> GetMultipleAsync(CancellationToken cancellationToken)
    {
        try
        {
            var query = _container.GetItemQueryIterator<PermitApplication>();
            var results = new List<PermitApplication>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync(cancellationToken);
                results.AddRange(response.ToList());
            }

            return results;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve all permit applications: {cex.Message}");
            throw new Exception("An error occur while trying to retrieve all permit applications.");
        }
    }

    public async Task<IEnumerable<SummarizedPermitApplication>> GetAllApplicationsAsync(CancellationToken cancellationToken)
    {
        try
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
            "a.Application.AppointmentDateTime as AppointmentDateTime, " +
            "a.Application.IsComplete as IsComplete, " +
            "a.Application.OrderId as OrderId, " +
            "a.id " +
            "FROM a"
        );

            var results = new List<SummarizedPermitApplication>();
            using (var appsIterator = _container.GetItemQueryIterator<SummarizedPermitApplication>(query))
            {
                while (appsIterator.HasMoreResults)
                {
                    FeedResponse<SummarizedPermitApplication> apps = await appsIterator.ReadNextAsync(cancellationToken);
                    foreach (var item in apps)
                    {
                        results.Add(item);
                    }
                }
            }

            return results;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve all summarized permit applications: {cex.Message}");
            throw new Exception("An error occur while trying to retrieve all summarized permit applications.");
        }
    }

    public async Task UpdateAsync(PermitApplication application, CancellationToken cancellationToken)
    {
        try
        {
            var modelS = JsonConvert.SerializeObject(application.History[0]);
            var model = JsonConvert.DeserializeObject<History>(modelS);
            var history = new History
            {
                ChangeMadeBy = model.ChangeMadeBy,
                Change = model.Change,
                ChangeDateTimeUtc = model.ChangeDateTimeUtc,
            };

            var x = await _container.PatchItemAsync<PermitApplication>(
                application.Id.ToString(),
                new PartitionKey(application.Id.ToString()),
                new[]
                {
                    PatchOperation.Set("/Application", application.Application),
                    PatchOperation.Add("/History/-", history)
                },
                null,
                cancellationToken
            );
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to update permit application: {cex.Message}");
            throw new Exception("An error occur while trying to update permit application.");
        }
    }

    public async Task<IEnumerable<History>> GetApplicationHistoryAsync(string applicationIdOrOrderId, CancellationToken cancellationToken, bool isOrderId = false)
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
                FeedResponse<HistoryResponse> response = await filteredFeed.ReadNextAsync(cancellationToken);

                var history = response.Resource.ToList();

                for (int i = 0; i < history[0].History.Length; i++)
                {
                    result.Add(history[0].History[i]);
                }

                return result;
            }

            return new List<History>();
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve permit application history: {cex.Message}");
            throw new Exception("An error occur while trying to retrieve permit application history.");
        }
    }
}

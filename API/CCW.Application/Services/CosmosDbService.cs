using CCW.Application.Entities;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Container = Microsoft.Azure.Cosmos.Container;


namespace CCW.Application.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public async Task<PermitApplication> AddAsync(PermitApplication application, CancellationToken cancellationToken)
    {
        PermitApplication createdItem = await _container.CreateItemAsync(application, new PartitionKey(application.Id.ToString()), null, cancellationToken);
        return createdItem;
    }

    public async Task<PermitApplication?> GetLastApplicationAsync(string userEmailOrOrderId, bool isOrderId, bool isComplete, CancellationToken cancellationToken)
    {
        var queryString = isOrderId
            ? "SELECT a.Application, a.id, a.History FROM applications a " +
              "WHERE a.Application.OrderId = @userEmailOrOrderId and a.Application.IsComplete = @isComplete " +
              "Order by a.OrderId DESC"
            : "SELECT a.Application, a.id, a.History FROM applications a " +
              "WHERE a.Application.UserEmail = @userEmailOrOrderId and a.Application.IsComplete = @isComplete " +
              "Order by a.OrderId DESC";

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

    public async Task<IEnumerable<PermitApplication>> GetAllUserApplicationsAsync(string userEmail, CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id FROM applications a " +
                          "WHERE a.Application.UserEmail = @userEmail " +
                          "Order by a.OrderId DESC";

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

        return new List<PermitApplication>();
    }

    public async Task<IEnumerable<History>> GetApplicationHistoryAsync(string applicationIdOrOrderId, CancellationToken cancellationToken, bool isOrderId = false)
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
        }

        return result;
    }

    public async Task<IEnumerable<SummarizedPermitApplication>> GetAllApplicationsAsync(CancellationToken cancellationToken)
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
            "a.Application.DOB as DOB, " +
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

    public static bool IsDOBValue(string s)
    {
        foreach (char c in s)
        {
            if (c == '-' || c == '/')
                return true;
        }
        return false;
    }

    public async Task<IEnumerable<SummarizedPermitApplication>> SearchApplicationsAsync(string searchValue, CancellationToken cancellationToken)
    {
        //string whereClause = string.Empty;

        //if (searchValue.All(Char.IsLetter))
        //{
        //    whereClause = "CONTAINS(@searchValue, a.Application.PersonalInfo.LastName) and " +
        //                  "(a.Application.PersonalInfo.LastName != '' or a.Application.PersonalInfo.LastName != null";
        //}
        //else if (searchValue.All(Char.IsDigit))
        //{
        //    whereClause = "CONTAINS(@searchValue, a.Application.PersonalInfo.Ssn) and " +
        //                  "a.Application.PersonalInfo.Ssn != '' or a.Application.PersonalInfo.Ssn != null";
        //}
        //else
        //{
        //    whereClause = "CONTAINS(@searchValue, a.Application.PersonalInfo.Ssn) and " +
        //                  "a.Application.PersonalInfo.Ssn != '' or a.Application.PersonalInfo.Ssn != null";
        //}

        var queryString =
            "SELECT " +
            "a.Application.UserEmail as UserEmail, " +
            "a.Application.CurrentAddress as CurrentAddress, " +
            "a.Application.PersonalInfo.LastName as LastName, " +
            "a.Application.PersonalInfo.FirstName as FirstName, " +
            "a.Application.ApplicationStatus as ApplicationStatus, " +
            "a.Application.AppointmentStatus as AppointmentStatus, " +
            "a.Application.AppointmentDateTime as AppointmentDateTime, " +
            "a.Application.IsComplete as IsComplete, " +
            "a.Application.DOB as DOB, " +
            "a.Application.OrderId as OrderId, " +
            "a.id " +
            "FROM a " +
            "WHERE " +
               "CONTAINS(@searchValue, a.Application.Contact.CellPhoneNumber) or " +
               "CONTAINS(@searchValue, a.Application.IdInfo.IdNumber) or " +
               "CONTAINS(@searchValue, a.Application.MailingAddress.AddressLine1) or " +
               "CONTAINS(@searchValue, a.Application.CurrentAddress.AddressLine1) or " +
               "CONTAINS(@searchValue, a.Application.PersonalInfo.LastName) or" +
               "CONTAINS(@searchValue, a.Application.PersonalInfo.FirstName) or " +
               "CONTAINS(@searchValue, a.Application.PersonalInfo.Ssn) or " +
               "CONTAINS(@searchValue, a.Application.UserEmail) or " +
               "CONTAINS(@searchValue, a.Application.DOB.BirthDate)";
                             

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@searchValue", searchValue);

        using FeedIterator<SummarizedPermitApplication> filteredFeed = _container.GetItemQueryIterator<SummarizedPermitApplication>(
            queryDefinition: parameterizedQuery
        );
        var results = new List<SummarizedPermitApplication>();
        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<SummarizedPermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);
            foreach (var item in response)
            {
                results.Add(item);
            }
            return response.Resource;

        }
        return results;
    }

    public async Task UpdateUserApplicationAsync(PermitApplication application, CancellationToken cancellationToken)
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
            },
            null,
            cancellationToken
        );
    }

    public async Task UpdateApplicationAsync(PermitApplication application, CancellationToken cancellationToken)
    {
        await _container.PatchItemAsync<PermitApplication>(
            application.Id.ToString(),
            new PartitionKey(application.Id.ToString()),
            new[]
            {
                PatchOperation.Set("/Application", application.Application)
            },
            null,
            cancellationToken
        );
    }

    public async Task DeleteUserApplicationAsync(string applicationId, CancellationToken cancellationToken)
    {
        await _container.DeleteItemAsync<PermitApplication>(applicationId, new PartitionKey(applicationId), cancellationToken: cancellationToken);
    }

    public async Task DeleteApplicationAsync(string applicationId, CancellationToken cancellationToken)
    {
        await _container.DeleteItemAsync<PermitApplication>(applicationId, new PartitionKey(applicationId), cancellationToken:cancellationToken);
    }
}
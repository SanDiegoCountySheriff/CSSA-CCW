using CCW.Application.Entities;
using iText.Layout.Borders;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using Container = Microsoft.Azure.Cosmos.Container;


namespace CCW.Application.Services;

public class CosmosDbService : ICosmosDbService
{
    private static Random random = new Random();
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
        application.Application.OrderId = GetPrefixLetter() + GetGeneratedTime() + RandomString();
        PermitApplication createdItem = await _container.CreateItemAsync(application, new PartitionKey(application.UserId), null, cancellationToken);
        return createdItem;
    }

    public async Task<IEnumerable<PermitApplication>> GetAllOpenApplicationsForUserAsync(string userId,
        CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a " +
                          "WHERE a.userId = @userId and a.Application.IsComplete = false ";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@userId", userId);

        using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery,
            requestOptions: new QueryRequestOptions() { PartitionKey = new PartitionKey(userId) }
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            return response.Resource;
        }

        return new List<PermitApplication>();
    }

    public async Task<string> GetSSNAsync(string userId, CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId FROM applications a " +
                          "WHERE a.userId = @userId " +
                          "Order by a.Application.OrderId DESC";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@userId", userId);

        using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery,
            requestOptions: new QueryRequestOptions() { PartitionKey = new PartitionKey(userId) }
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            var application = response.Resource.FirstOrDefault();

            if (application != null && application.Application.PersonalInfo != null)
            {
                return application.Application.PersonalInfo.Ssn;
            }
        }

        return string.Empty;
    }

    public async Task<PermitApplication?> GetLastApplicationAsync(string userId, string applicationId,
        CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a " +
                          "WHERE a.userId = @userId and a.id = @applicationId " +
                          "Order by a.Application.OrderId DESC";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@userId", userId)
            .WithParameter("@applicationId", applicationId);

        using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery,
            requestOptions: new QueryRequestOptions() { PartitionKey = new PartitionKey(userId) }
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            var application = response.Resource.FirstOrDefault();

            return application;
        }

        return null!;
    }

    public async Task<PermitApplication?> GetUserLastApplicationAsync(string userEmailOrOrderId, bool isOrderId,
        bool isComplete, CancellationToken cancellationToken)
    {
        var queryString = isOrderId
            ? "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a " +
              "WHERE a.Application.OrderId = @userEmailOrOrderId and a.Application.IsComplete = @isComplete " +
              "Order by a.Application.OrderId DESC"
            : "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a " +
              "WHERE a.Application.UserEmail = @userEmailOrOrderId and a.Application.IsComplete = @isComplete " +
              "Order by a.Application.OrderId DESC";

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

    public async Task<IEnumerable<PermitApplication>> GetMultipleApplicationsAsync(string[] applicationsIds,
        CancellationToken cancellationToken)
    {
        var orderIdsList = applicationsIds.ToList();
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a " +
            "WHERE ARRAY_CONTAINS(@orderIdsList, a.Application.OrderId)";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@orderIdsList", orderIdsList);

        var applications = new List<PermitApplication>();

        using var feedIterator = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery
        );

        while (feedIterator.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await feedIterator.ReadNextAsync(cancellationToken);
            applications.AddRange(response);
        }
        
        return applications;
    }

    public async Task<IEnumerable<PermitApplication>> GetAllApplicationsAsync(string userId, string userEmail,
        CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory FROM applications a " +
                          "WHERE a.userId = @userId and a.Application.UserEmail = @userEmail " +
                          "Order by a.Application.OrderId DESC";

        var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@userId", userId)
                .WithParameter("@userEmail", userEmail);


        using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery,
            requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId) }
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            return response.Resource;
        }

        return new List<PermitApplication>();
    }

    public async Task<IEnumerable<PermitApplication>> GetAllUserApplicationsAsync(string userEmail,
        CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a " +
                          "WHERE a.Application.UserEmail = @userEmail " +
                          "Order by a.Application.OrderId DESC";

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

    public async Task<PermitApplication?> GetUserApplicationAsync(string applicationId, CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History FROM applications a " +
                          "WHERE a.id = @applicationId ";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@applicationId", applicationId);

        using FeedIterator<PermitApplication> filteredFeed = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            return response.Resource.FirstOrDefault();
        }

        return null!;
    }

    public async Task<IEnumerable<History>> GetApplicationHistoryAsync(string applicationIdOrOrderId,
        CancellationToken cancellationToken, bool isOrderId = false)
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

    public async Task<IEnumerable<SummarizedPermitApplication>> GetAllInProgressApplicationsSummarizedAsync(
        CancellationToken cancellationToken)
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
            "a.Application.ApplicationType as ApplicationType, " +
            "a.Application.IsComplete as IsComplete, " +
            "a.Application.DOB as DOB, " +
            "a.Application.OrderId as OrderId, " +
            "a.userId as UserId, " +
            "a.Application.AssignedTo as AssignedTo," +
            "a.Application.FlaggedForLicensingReview as FlaggedForLicensingReview," +
            "a.Application.FlaggedForCustomerReview as FlaggedForCustomerReview," +
            "a.id " +
            "FROM a "
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

        return results.OrderByDescending(a => a.IsComplete);
    }

    public async Task<IEnumerable<SummarizedPermitApplication>> SearchApplicationsAsync(string searchValue,
        CancellationToken cancellationToken)
    {
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
                "a.userId as UserId, " +
                "a.Application.OrderId as OrderId, " +
                "a.id " +
                "FROM a " +
                "WHERE " +
                "CONTAINS(a.Application.Contact.PrimaryPhoneNumber, @searchValue, true) or " +
                "CONTAINS(a.Application.Contact.CellPhoneNumber, @searchValue, true) or " +
                "CONTAINS(a.Application.IdInfo.IdNumber, @searchValue, true) or " +
                "CONTAINS(a.Application.MailingAddress.AddressLine1, @searchValue, true) or " +
                "CONTAINS(a.Application.CurrentAddress.AddressLine1, @searchValue, true) or " +
                "CONTAINS(a.Application.PersonalInfo.LastName, @searchValue, true) or " +
                "CONTAINS(a.Application.PersonalInfo.FirstName, @searchValue, true) or " +
                "CONTAINS(a.Application.PersonalInfo.Ssn, @searchValue, true) or " +
                "CONTAINS(a.Application.DOB.BirthDate, @searchValue, true) or " +
                "CONTAINS(a.Application.UserEmail, @searchValue, true)";


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

    public async Task UpdateApplicationAsync(PermitApplication application, PermitApplication existingApplication, CancellationToken cancellationToken)
    {
        application.Application.Comments = existingApplication.Application.Comments;
        application.Application.BackgroundCheck = existingApplication.Application.BackgroundCheck;

        await _container.PatchItemAsync<PermitApplication>(
            application.Id.ToString(),
            new PartitionKey(application.UserId),
            new[]
            {
                PatchOperation.Set("/Application", application.Application)
            },
            null,
            cancellationToken
        );
    }

    public async Task UpdateUserApplicationAsync(PermitApplication application, CancellationToken cancellationToken)
    {
        List<PatchOperation> patches = new List<PatchOperation>(3);
        patches.Add(PatchOperation.Set("/Application", application.Application));

        var modelS = JsonConvert.SerializeObject(application.History[0]);
        var model = JsonConvert.DeserializeObject<History>(modelS);
        var history = new History
        {
            ChangeMadeBy = model.ChangeMadeBy,
            Change = model.Change,
            ChangeDateTimeUtc = model.ChangeDateTimeUtc,
        };
        patches.Add(PatchOperation.Add("/History/-", history));

        if (null != application.PaymentHistory && application.PaymentHistory.Length > 0)
        {
            int paymentHistoryCount = application.PaymentHistory.Length;
            PaymentHistory[] paymentHistories = new PaymentHistory[paymentHistoryCount];

            for (int i = 0; i < paymentHistoryCount; i++)
            {
                var modelSPayment = JsonConvert.SerializeObject(application.PaymentHistory[i]);
                var modelPayment = JsonConvert.DeserializeObject<PaymentHistory>(modelSPayment);
                var paymentHistory = new PaymentHistory
                {

                    PaymentDateTimeUtc = modelPayment.PaymentDateTimeUtc,
                    PaymentType = modelPayment.PaymentType,
                    VendorInfo = modelPayment.VendorInfo,
                    Amount = modelPayment.Amount,
                    RecordedBy = modelPayment.RecordedBy,
                    TransactionId = modelPayment.TransactionId,
                };

                paymentHistories[i] = paymentHistory;
            }

            patches.Add(PatchOperation.Replace("/PaymentHistory", paymentHistories));
        }

        await _container.PatchItemAsync<PermitApplication>(
            application.Id.ToString(),
            new PartitionKey(application.UserId),
            patches,
            null,
            cancellationToken
        );
    }

    public async Task DeleteApplicationAsync(string userId, string applicationId, CancellationToken cancellationToken)
    {
        await _container.DeleteItemAsync<PermitApplication>(applicationId, new PartitionKey(userId), cancellationToken: cancellationToken);
    }

    public async Task DeleteUserApplicationAsync(string userId, string applicationId, CancellationToken cancellationToken)
    {
        await _container.DeleteItemAsync<PermitApplication>(applicationId, new PartitionKey(userId), cancellationToken: cancellationToken);
    }

    private string GetGeneratedTime()
    {
        var result = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")
                     + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

        return result;
    }

    private static char GetPrefixLetter()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int num = random.Next(0, chars.Length);
        return chars[num];
    }

    private string RandomString()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var stringChars = new char[3];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new String(stringChars);
    }
}
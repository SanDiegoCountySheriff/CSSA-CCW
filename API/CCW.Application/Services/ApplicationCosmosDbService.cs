using CCW.Application.Services.Contracts;
using CCW.Common.Enums;
using CCW.Common.Models;
using CCW.Common.ResponseModels;
using Microsoft.Azure.Cosmos;
using static CCW.Application.Controllers.PermitApplicationController;

namespace CCW.Application.Services;

public class ApplicationCosmosDbService : IApplicationCosmosDbService
{
    private static readonly Random random = new();
    private readonly Container _container;
    private readonly Container _historicalContainer;
    private readonly Container _legacyContainer;

    public ApplicationCosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName,
        string legacyContainerName,
        string historicalContainerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
        _historicalContainer = cosmosDbClient.GetContainer(databaseName, historicalContainerName);
        _legacyContainer = cosmosDbClient.GetContainer(databaseName, legacyContainerName);
    }

    public async Task<PermitApplication> AddAsync(PermitApplication application, CancellationToken cancellationToken)
    {
        application.Application.OrderId = GetPrefixLetter() + GetGeneratedTime() + RandomString();
        PermitApplication createdItem = await _container.CreateItemAsync(application, new PartitionKey(application.UserId), null, cancellationToken);

        return createdItem;
    }

    public async Task<PermitApplication> AddHistoricalApplicationAsync(PermitApplication application, CancellationToken cancellationToken)
    {
        application.Id = Guid.NewGuid();
        application.HistoricalDate = DateTimeOffset.UtcNow;
        PermitApplication createdItem = await _historicalContainer.CreateItemAsync(application, new PartitionKey(application.UserId), null, cancellationToken);

        return createdItem;
    }

    public async Task<PermitApplication> AddHistoricalApplicationPublicAsync(PermitApplication application, PermitApplication existingApplication, CancellationToken cancellationToken)
    {
        application.Id = Guid.NewGuid();
        application.HistoricalDate = DateTimeOffset.UtcNow;

        application.History = existingApplication.History;
        application.Application.BackgroundCheck = existingApplication.Application.BackgroundCheck;
        application.Application.Comments = existingApplication.Application.Comments;
        application.Application.ReferenceNotes = existingApplication.Application.ReferenceNotes;

        PermitApplication createdItem = await _historicalContainer.CreateItemAsync(application, new PartitionKey(application.UserId), null, cancellationToken);

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

    public async Task<PermitApplication> GetLastApplicationAsync(string userId, string applicationId,
        CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History, a.IsMatchUpdated FROM applications a " +
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

    public async Task<PermitApplication> GetHistoricalApplication(string id, CancellationToken cancellationToken)
    {
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History, a.IsMatchUpdated, a.HistoricalDate FROM applications a WHERE a.id = @id Order by a.Application.OrderId DESC";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@id", id);

        using FeedIterator<PermitApplication> filteredFeed = _historicalContainer.GetItemQueryIterator<PermitApplication>(queryDefinition: parameterizedQuery);

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            var application = response.Resource.FirstOrDefault();

            if (!string.IsNullOrEmpty(application.Application.PersonalInfo.Ssn))
            {
                string ssn = application.Application.PersonalInfo.Ssn;
                string maskedSsn = string.Concat(new string('X', ssn.Length - 4), ssn.AsSpan(ssn.Length - 4));
                application.Application.PersonalInfo.Ssn = maskedSsn;
            }

            return application;
        }

        return null!;
    }

    public async Task<PermitApplication> GetUserLastApplicationAsync(string userEmailOrOrderId, bool isOrderId,
        bool isComplete, bool isLegacy, CancellationToken cancellationToken)
    {
        var queryString = isOrderId
            ? "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History, a.IsMatchUpdated FROM applications a " +
              "WHERE a.Application.OrderId = @userEmailOrOrderId " +
              "Order by a.Application.OrderId DESC"
            : "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.History, a.IsMatchUpdated FROM applications a " +
              "WHERE a.Application.UserEmail = @userEmailOrOrderId " +
              "Order by a.Application.OrderId DESC";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@userEmailOrOrderId", userEmailOrOrderId);

        using FeedIterator<PermitApplication> filteredFeed =
            isLegacy ?
            _legacyContainer.GetItemQueryIterator<PermitApplication>(queryDefinition: parameterizedQuery) :
            _container.GetItemQueryIterator<PermitApplication>(queryDefinition: parameterizedQuery);

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            var application = response.Resource.FirstOrDefault();

            if (!string.IsNullOrEmpty(application.Application.PersonalInfo.Ssn))
            {
                string ssn = application.Application.PersonalInfo.Ssn;
                string maskedSsn = string.Concat(new string('X', ssn.Length - 4), ssn.AsSpan(ssn.Length - 4));
                application.Application.PersonalInfo.Ssn = maskedSsn;
            }

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
        var queryString = "SELECT a.Application, a.id, a.userId, a.PaymentHistory, a.IsMatchUpdated FROM applications a " +
                          "WHERE a.userId = @userId " +
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

    public async Task<PermitApplication> GetUserApplicationAsync(string applicationId, CancellationToken cancellationToken)
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

    public async Task<(IEnumerable<SummarizedPermitApplication>, int)> GetAllInProgressApplicationsSummarizedAsync(PermitsOptions options, CancellationToken cancellationToken)
    {
        var count = await GetApplicationCountAsync(options, cancellationToken);

        int totalPages = count / options.ItemsPerPage;
        var isBeyondLastPage = options.Page > totalPages + 1;

        while (isBeyondLastPage && options.Page > 1)
        {
            options.Page -= 1;
            isBeyondLastPage = options.Page > totalPages;
        }

        QueryDefinition query = GetQueryDefinition(options);

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

        return (results, count);
    }

    public async Task<List<string>> GetEmailsAsync(PermitsOptions options, CancellationToken cancellationToken)
    {
        QueryDefinition query = GetLegacyQueryDefinition(options, false, true);

        var results = new List<string>();

        using (var appsIterator = _legacyContainer.GetItemQueryIterator<string>(query))
        {
            while (appsIterator.HasMoreResults)
            {
                FeedResponse<string> emails = await appsIterator.ReadNextAsync(cancellationToken);

                foreach (var item in emails)
                {
                    results.Add(item);
                }
            }
        }

        return results;
    }

    public async Task<(IEnumerable<SummarizedLegacyApplication>, int)> GetAllLegacyApplicationsAsync(PermitsOptions options, CancellationToken cancellationToken)
    {
        var count = await GetLegacyApplicationCount(options, cancellationToken);

        int totalPages = count / options.ItemsPerPage;
        var isBeyondLastPage = options.Page > totalPages + 1;

        while (isBeyondLastPage && options.Page > 1)
        {
            options.Page -= 1;
            isBeyondLastPage = options.Page > totalPages;
        }

        QueryDefinition query = GetLegacyQueryDefinition(options);

        var results = new List<SummarizedLegacyApplication>();

        using (var appsIterator = _legacyContainer.GetItemQueryIterator<SummarizedLegacyApplication>(query))
        {
            while (appsIterator.HasMoreResults)
            {
                FeedResponse<SummarizedLegacyApplication> apps = await appsIterator.ReadNextAsync(cancellationToken);

                foreach (var item in apps)
                {
                    results.Add(item);
                }
            }
        }

        return (results, count);
    }

    public async Task<int> GetApplicationHistoricalCount(string orderId, CancellationToken cancellationToken)
    {
        var queryString = "SELECT VALUE Count(1) FROM c WHERE c.Application.OrderId = @orderId";
        var parameterizedQuery = new QueryDefinition(queryString).WithParameter("@orderId", orderId);

        using FeedIterator<int> filteredFeed = _historicalContainer.GetItemQueryIterator<int>(
            queryDefinition: parameterizedQuery
        );

        if (filteredFeed.HasMoreResults)
        {
            var result = await filteredFeed.ReadNextAsync();

            return result.Resource.FirstOrDefault();
        }

        return 0;
    }

    public async Task<List<HistoricalApplicationSummary>> GetHistoricalApplicationSummary(string orderId, CancellationToken cancellationToken)
    {
        var queryString = "SELECT c.id, c.HistoricalDate, c.Application.ApplicationType FROM c WHERE c.Application.OrderId = @orderId";
        var query = new QueryDefinition(queryString).WithParameter("@orderId", orderId);

        var result = new List<HistoricalApplicationSummary>();

        using FeedIterator<HistoricalApplicationSummary> feedIterator = _historicalContainer.GetItemQueryIterator<HistoricalApplicationSummary>(queryDefinition: query);

        if (feedIterator.HasMoreResults)
        {
            var response = await feedIterator.ReadNextAsync();

            foreach (var item in response)
            {
                result.Add(item);
            }
        }

        return result;
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

    public async Task<IEnumerable<SummarizedPermitApplicationReport>> GetPermitsByDateAsync(DateTime date, CancellationToken cancellationToken)
    {
        var queryString = @"SELECT a.Application, a.id, a.Application.OrderId, a.PaymentHistory, a.Application.PersonalInfo.FirstName, 
                          a.Application.PersonalInfo.LastName, a.Application.AppointmentDateTime, a.Application.PersonalInfo.MiddleName,
                          a.Application.PersonalInfo.Suffix, a.Application.DOB.BirthDate, a.Application.Aliases
                          FROM applications a WHERE STARTSWITH(a.Application.AppointmentDateTime, @date)";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@date", date.ToString("yyyy-MM-dd"));

        var results = new List<SummarizedPermitApplicationReport>();

        using FeedIterator<SummarizedPermitApplicationReport> iterator = _container.GetItemQueryIterator<SummarizedPermitApplicationReport>(
            queryDefinition: parameterizedQuery
        );

        while (iterator.HasMoreResults)
        {
            FeedResponse<SummarizedPermitApplicationReport> response = await iterator.ReadNextAsync(cancellationToken);

            foreach (var item in response)
            {
                results.Add(item);
            }
        }

        return results;
    }

    public async Task UpdateApplicationAsync(PermitApplication application, PermitApplication existingApplication, CancellationToken cancellationToken)
    {
        application.Application.Comments = existingApplication.Application.Comments;
        application.Application.BackgroundCheck = existingApplication.Application.BackgroundCheck;
        application.History = existingApplication.History;

        if (existingApplication.Application.ApplicationType != application.Application.ApplicationType &&
            application.Application.ApplicationType is
            ApplicationType.RenewStandard or
            ApplicationType.RenewJudicial or
            ApplicationType.RenewReserve or
            ApplicationType.RenewEmployment
        )
        {
            application.Application.BackgroundCheck.ProofOfID = new BackgroundCheckItem();
            application.Application.BackgroundCheck.ProofOfResidency = new BackgroundCheckItem();
            application.Application.BackgroundCheck.NCICWantsWarrants = new BackgroundCheckItem();
            application.Application.BackgroundCheck.Locals = new BackgroundCheckItem();
            application.Application.BackgroundCheck.DMVRecord = new BackgroundCheckItem();
            application.Application.BackgroundCheck.AKAsChecked = new BackgroundCheckItem();
            application.Application.BackgroundCheck.CrimeTracer = new BackgroundCheckItem();
            application.Application.BackgroundCheck.TrafficCourtPortal = new BackgroundCheckItem();
            application.Application.BackgroundCheck.Livescan = new BackgroundCheckItem();
            application.Application.BackgroundCheck.SR14 = new BackgroundCheckItem();
            application.Application.BackgroundCheck.Firearms = new BackgroundCheckItem();
            application.Application.BackgroundCheck.SafetyCertificate = new BackgroundCheckItem();
            application.Application.BackgroundCheck.Restrictions = new BackgroundCheckItem();
            application.Application.Comments = Array.Empty<Comment>();
            application.History = Array.Empty<History>();
            application.PaymentHistory = new List<PaymentHistory>();

            if (application.Application.LegacyQualifyingQuestions != null)
            {
                application.Application.QualifyingQuestions = new QualifyingQuestions
                {
                    QuestionOne = new QualifyingQuestionOne(),
                    QuestionTwo = new QualifyingQuestionTwo(),
                    QuestionThree = new QualifyingQuestionStandard(),
                    QuestionFour = new QualifyingQuestionStandard(),
                    QuestionFive = new QualifyingQuestionStandard(),
                    QuestionSix = new QualifyingQuestionStandard(),
                    QuestionSeven = new QualifyingQuestionStandard(),
                    QuestionEight = new QualifyingQuestionStandard(),
                    QuestionNine = new QualifyingQuestionStandard(),
                    QuestionTen = new QualifyingQuestionStandard(),
                    QuestionEleven = new QualifyingQuestionStandard(),
                    QuestionTwelve = new QualifyingQuestionTwelve(),
                    QuestionThirteen = new QualifyingQuestionStandard(),
                    QuestionFourteen = new QualifyingQuestionStandard(),
                    QuestionFifteen = new QualifyingQuestionStandard(),
                    QuestionSixteen = new QualifyingQuestionStandard(),
                    QuestionSeventeen = new QualifyingQuestionStandard(),
                    QuestionEighteen = new QualifyingQuestionStandard(),
                    QuestionNineteen = new QualifyingQuestionStandard(),
                    QuestionTwenty = new QualifyingQuestionStandard(),
                    QuestionTwentyOne = new QualifyingQuestionStandard(),
                };
                application.Application.IsRenewingWithLegacyQuestions = true;
                application.Application.LegacyQualifyingQuestions = null;

            }
        }

        if (existingApplication.Application.ApplicationType != application.Application.ApplicationType &&
            application.Application.ApplicationType is
            ApplicationType.ModifyEmployment or
            ApplicationType.ModifyJudicial or
            ApplicationType.ModifyReserve or
            ApplicationType.ModifyStandard
        )
        {
            if (application.Application.UploadedDocuments.Any(doc =>
            {
                return doc.DocumentType == "ModifyName";
            })
            )
            {
                application.Application.ModifiedNameComplete = false;
                application.Application.BackgroundCheck.ProofOfID = new BackgroundCheckItem();
            }

            if (application.Application.UploadedDocuments.Any(doc =>
            {
                return doc.DocumentType == "ModifyAddress";
            })
            )
            {
                application.Application.ModifiedAddressComplete = false;
                application.Application.BackgroundCheck.ProofOfResidency = new BackgroundCheckItem();
            }

            if (application.Application.UploadedDocuments.Any(doc =>
            {
                return doc.DocumentType == "ModifyWeapons";
            })
            )
            {
                application.Application.ModifiedWeaponComplete = false;
                application.Application.BackgroundCheck.Firearms = new BackgroundCheckItem();
                application.Application.BackgroundCheck.SafetyCertificate = new BackgroundCheckItem();
            }
        }

        await _container.UpsertItemAsync(application, new PartitionKey(application.UserId), null, cancellationToken);
    }

    public async Task UpdateUserApplicationAsync(PermitApplication application, CancellationToken cancellationToken)
    {
        await _container.UpsertItemAsync(application, new PartitionKey(application.UserId), null, cancellationToken);
    }

    public async Task DeleteUserApplicationAsync(string userId, string applicationId, CancellationToken cancellationToken)
    {
        await _container.DeleteItemAsync<PermitApplication>(applicationId, new PartitionKey(userId), cancellationToken: cancellationToken);
    }

    public async Task<int> GetApplicationCountAsync(PermitsOptions options, CancellationToken cancellationToken)
    {
        QueryDefinition query = GetQueryDefinition(options, true);

        using FeedIterator<int> filteredFeed = _container.GetItemQueryIterator<int>(
            queryDefinition: query
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<int> response = await filteredFeed.ReadNextAsync(cancellationToken);

            return response.Resource.FirstOrDefault();
        }

        return 0;
    }

    public async Task WithdrawRenewal(string userId, CancellationToken cancellationToken)
    {
        var queryString = "SELECT * FROM c WHERE c.userId = @userId ORDER BY c._ts DESC";

        var parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@userId", userId);

        using FeedIterator<PermitApplication> historicalResult = _historicalContainer.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery
        );

        PermitApplication historicalApplication = null;

        if (historicalResult.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await historicalResult.ReadNextAsync(cancellationToken);

            historicalApplication = response.Resource.FirstOrDefault();
        }

        queryString = "SELECT * FROM c WHERE c.userId = @userId ORDER BY c._ts DESC";

        parameterizedQuery = new QueryDefinition(query: queryString)
            .WithParameter("@userId", userId);

        using FeedIterator<PermitApplication> productionResult = _container.GetItemQueryIterator<PermitApplication>(
            queryDefinition: parameterizedQuery
        );

        PermitApplication existingApplication = null;

        if (productionResult.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await productionResult.ReadNextAsync(cancellationToken);

            existingApplication = response.Resource.FirstOrDefault();
        }

        if (existingApplication != null)
        {
            await _container.DeleteItemAsync<PermitApplication>(existingApplication.Id.ToString(), new PartitionKey(existingApplication.UserId), cancellationToken: default);
        }

        if (historicalApplication != null)
        {
            var history = new History()
            {
                Change = "Withdraw Renewal",
                ChangeDateTimeUtc = DateTimeOffset.UtcNow,
                ChangeMadeBy = "Customer Action"
            };

            historicalApplication.History = historicalApplication.History.Append(history).ToArray();

            await _container.CreateItemAsync(historicalApplication, new PartitionKey(userId), cancellationToken: default);

            await _historicalContainer.DeleteItemAsync<PermitApplication>(historicalApplication.Id.ToString(), new PartitionKey(userId), cancellationToken: default);
        }
    }

    public async Task<int> GetLegacyApplicationCount(PermitsOptions options, CancellationToken cancellationToken)
    {
        QueryDefinition query = GetLegacyQueryDefinition(options, true);

        using FeedIterator<int> filteredFeed = _legacyContainer.GetItemQueryIterator<int>(
            queryDefinition: query
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<int> response = await filteredFeed.ReadNextAsync(cancellationToken);

            return response.Resource.FirstOrDefault();
        }

        return 0;
    }

    public async Task<ApplicationSummaryCountResponseModel> GetApplicationSummaryCount(CancellationToken cancellationToken)
    {
        var queryString = "SELECT Count(c.Application.ApplicationType = 1 ? 1 : undefined) as standardType, Count(c.Application.ApplicationType = 2 ? 1 : undefined) as reserveType, Count(c.Application.ApplicationType = 3 ? 1 : undefined) as judicialType, Count(c.Application.ApplicationType = 4 ? 1 : undefined) as employmentType, Count(c.Application.Status = 9 ? 1 : undefined) as suspendedStatus, Count(c.Application.Status = 10 ? 1 : undefined) as revokedStatus, Count(c.Application.Status = 12 ? 1 : undefined) as deniedStatus, Count(c.Application.Status = 8 AND c.Application.ApplicationType = 1 ? 1 : undefined) as activeStandardStatus, Count(c.Application.Status = 8 AND c.Application.ApplicationType = 2 ? 1 : undefined) as activeReserveStatus, Count(c.Application.Status = 8 AND c.Application.ApplicationType = 3 ? 1 : undefined) as activeJudicialStatus, Count(c.Application.Status = 8 AND c.Application.ApplicationType = 4 ? 1 : undefined) as activeEmploymentStatus, Count(c.Application.Status = 2 ? 1 : undefined) as submittedStatus FROM c";
        var query = new QueryDefinition(queryString);

        var result = new ApplicationSummaryCountResponseModel();

        using FeedIterator<ApplicationSummaryCountResponseModel> filteredFeed = _container.GetItemQueryIterator<ApplicationSummaryCountResponseModel>(
            queryDefinition: query
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<ApplicationSummaryCountResponseModel> response = await filteredFeed.ReadNextAsync(cancellationToken);

            result = response.Resource.FirstOrDefault();
        }

        return result;
    }

    public async Task<List<AssignedApplicationSummary>> GetAssignedApplicationsSummary(string userName, CancellationToken cancellationToken)
    {
        var queryString = "SELECT c.Application.OrderId as OrderId, CONCAT(c.Application.PersonalInfo.FirstName, \" \", c.Application.PersonalInfo.LastName) as Name, c.Application.Status as Status, c.Application.AppointmentStatus as AppointmentStatus FROM c WHERE c.Application.AssignedTo = @userName";
        var query = new QueryDefinition(queryString).WithParameter("@userName", userName);
        var result = new List<AssignedApplicationSummary>();

        using FeedIterator<AssignedApplicationSummary> filteredFeed = _container.GetItemQueryIterator<AssignedApplicationSummary>(queryDefinition: query);

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<AssignedApplicationSummary> response = await filteredFeed.ReadNextAsync(cancellationToken);

            return response.Resource.ToList();
        }

        return result;
    }

    private static string GetGeneratedTime()
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

    private static string RandomString()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var stringChars = new char[3];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new String(stringChars);
    }

    private static QueryDefinition GetQueryDefinition(PermitsOptions options, bool forCount = false)
    {
        var offset = 0;

        if (options.Page != 1)
        {
            offset = options.ItemsPerPage * (options.Page - 1);
        }

        var select = "SELECT a.Application.PersonalInfo.LastName as LastName, a.Application.PersonalInfo.FirstName as FirstName, a.Application.Status as Status, a.Application.AppointmentStatus as AppointmentStatus, a.Application.AppointmentDateTime as AppointmentDateTime, a.Application.AppointmentId as AppointmentId, a.Application.ApplicationType as ApplicationType, a.PaymentHistory as PaymentHistory, a.Application.IsComplete as IsComplete, a.Application.OrderId as OrderId, a.Application.AssignedTo as AssignedTo,a.Application.FlaggedForLicensingReview as FlaggedForLicensingReview,a.Application.FlaggedForCustomerReview as FlaggedForCustomerReview,a.id FROM a ";
        var where = "WHERE (a.Application.IsComplete = true OR a.Application.IsComplete = false) ";
        var order = "";
        var limit = "OFFSET @offset LIMIT @itemsPerPage";

        if (options.SortBy?.Length > 0)
        {
            var field = options.SortBy[0] switch
            {
                "email" => "a.Application.UserEmail",
                "idNumber" => "a.Application.IdInfo.IdNumber",
                "name" => "a.Application.PersonalInfo.LastName",
                "applicationType" => "a.Application.ApplicationType",
                "appointmentStatus" => "a.Application.AppointmentStatus",
                "appointmentDateTime" => "a.Application.AppointmentDateTime",
                "assignedTo" => "a.Application.AssignedTo",
                "status" => "a.Application.Status",
                _ => string.Empty
            };

            order += $" ORDER BY {field} ";

            if (options.SortDesc[0])
            {
                order += "DESC ";
            }
        }

        if (options.MatchedApplications)
        {
            where += "AND (a.IsMatchUpdated = true OR a.IsMatchUpdated = false) ";
        }

        if (options.Statuses is not null && !options.Statuses.Contains(ApplicationStatus.None))
        {
            where += "AND (";

            foreach (var status in options.Statuses)
            {
                where += $"a.Application.Status = {(int)status} OR ";
            }

            where = where.Remove(where.Length - 3);

            where += ") ";
        }

        if (options.AppointmentStatuses is not null)
        {
            where += "AND (";

            foreach (var status in options.AppointmentStatuses)
            {
                where += $"a.Application.AppointmentStatus = {(int)status} OR ";
            }

            where = where.Remove(where.Length - 3);

            where += ") ";
        }

        if (options.ApplicationTypes is not null && !options.ApplicationTypes.Contains(ApplicationType.None))
        {
            where += "AND (";

            foreach (var type in options.ApplicationTypes)
            {
                where += $"a.Application.ApplicationType = {(int)type} OR ";
            }

            where = where.Remove(where.Length - 3);

            where += ") ";
        }

        if (!string.IsNullOrEmpty(options.Search))
        {
            where += "AND (" +
                "CONTAINS(a.Application.PersonalInfo.LastName, @searchValue, true) OR " +
                "CONTAINS(a.Application.PersonalInfo.FirstName, @searchValue, true) OR " +
                "CONTAINS( a.Application.OrderId, @searchValue, true)) ";
        }

        if (options.ShowingTodaysAppointments || options.SelectedDate != null)
        {
            where += "AND SUBSTRING(a.Application.AppointmentDateTime, 0, 10) = @today";
        }

        var limitString = forCount ? string.Empty : limit;
        var selectString = forCount ? "SELECT VALUE Count(1) FROM a " : select;
        var orderString = forCount ? string.Empty : order;

        var queryString = $"{selectString} {where} {orderString} {limitString}";

        var queryDefinition = new QueryDefinition(queryString);

        if (!forCount)
        {
            queryDefinition.WithParameter("@offset", offset).WithParameter("@itemsPerPage", options.ItemsPerPage);
        }

        if (!string.IsNullOrEmpty(options.Search))
        {
            queryDefinition.WithParameter("@searchValue", options.Search);
        }

        if (options.ShowingTodaysAppointments || options.SelectedDate != null)
        {
            queryDefinition.WithParameter("@today", options.SelectedDate != null ? options.SelectedDate?.ToString("yyyy-MM-dd") : DateTime.UtcNow.Date.ToString("yyyy-MM-dd"));
        }

        return queryDefinition;
    }

    private static QueryDefinition GetLegacyQueryDefinition(PermitsOptions options, bool forCount = false, bool getEmails = false)
    {
        var offset = 0;

        if (options.Page != 1)
        {
            offset = options.ItemsPerPage * (options.Page - 1);
        }

        string select = string.Empty;

        if (!getEmails)
        {
            select = "SELECT a.Application.PersonalInfo.LastName as LastName, a.Application.PersonalInfo.FirstName as FirstName, a.Application.Status as Status, a.Application.AppointmentDateTime as AppointmentDateTime, a.Application.ApplicationType as ApplicationType, a.Application.OrderId as OrderId, a.Application.IdInfo.IdNumber as IdNumber, a.Application.DOB.BirthDate as BirthDate, a.Application.License.PermitNumber as PermitNumber, a.Application.UserEmail as Email, a.id FROM a ";
        }
        else if (getEmails)
        {
            select = "SELECT VALUE a.Application.UserEmail FROM a";
        }

        var where = "WHERE (a.Application.IsComplete = true OR a.Application.IsComplete = false) AND a.IsMatchUpdated = false ";
        var order = "";
        var limit = "OFFSET @offset LIMIT @itemsPerPage";

        if (options.SortBy?.Length > 0)
        {
            var field = options.SortBy[0] switch
            {
                "email" => "a.Application.UserEmail",
                "idNumber" => "a.Application.IdInfo.IdNumber",
                "name" => "a.Application.PersonalInfo.LastName",
                "applicationType" => "a.Application.ApplicationType",
                "appointmentStatus" => "a.Application.AppointmentStatus",
                "appointmentDateTime" => "a.Application.AppointmentDateTime",
                "assignedTo" => "a.Application.AssignedTo",
                "status" => "a.Application.Status",
                _ => string.Empty
            };

            order += $" ORDER BY {field} ";

            if (options.SortDesc[0])
            {
                order += "DESC ";
            }
        }

        if (options.Statuses is not null && !options.Statuses.Contains(ApplicationStatus.None))
        {
            where += "AND (";

            foreach (var status in options.Statuses)
            {
                where += $"a.Application.Status = {(int)status} OR ";
            }

            where = where.Remove(where.Length - 3);

            where += ") ";
        }

        if (getEmails)
        {
            where += $"AND a.Application.Status = 2 ";
        }

        if (options.AppointmentStatuses is not null)
        {
            where += "AND (";

            foreach (var status in options.AppointmentStatuses)
            {
                where += $"a.Application.AppointmentStatus = {(int)status} OR ";
            }

            where = where.Remove(where.Length - 3);

            where += ") ";
        }

        if (options.ApplicationTypes is not null && !options.ApplicationTypes.Contains(ApplicationType.None))
        {
            where += "AND (";

            foreach (var type in options.ApplicationTypes)
            {
                where += $"a.Application.ApplicationType = {(int)type} OR ";
            }

            where = where.Remove(where.Length - 3);

            where += ") ";
        }

        if (!string.IsNullOrEmpty(options.Search))
        {
            where += "AND (" +
                "CONTAINS(a.Application.PersonalInfo.LastName, @searchValue, true) OR " +
                "CONTAINS(a.Application.PersonalInfo.FirstName, @searchValue, true) OR " +
                "CONTAINS(a.Application.OrderId, @searchValue, true)) ";
        }

        if (!string.IsNullOrEmpty(options.ApplicationSearch))
        {
            where += "AND (" +
                "CONTAINS(a.Application.PersonalInfo.LastName, @applicationSearchValue, true) OR " +
                "CONTAINS(a.Application.PersonalInfo.FirstName, @applicationSearchValue, true) OR " +
                "CONTAINS(a.Application.IdInfo.IdNumber, @applicationSearchValue, true) OR " +
                "CONTAINS(a.Application.UserEmail, @applicationSearchValue, true)) ";
        }

        if (options.ShowingTodaysAppointments || options.SelectedDate != null)
        {
            where += "AND SUBSTRING(a.Application.AppointmentDateTime, 0, 10) = @today";
        }

        var limitString = forCount || getEmails ? string.Empty : limit;
        var selectString = forCount ? "SELECT VALUE Count(1) FROM a " : select;
        var orderString = forCount ? string.Empty : order;

        var queryString = $"{selectString} {where} {orderString} {limitString}";

        var queryDefinition = new QueryDefinition(queryString);

        if (!forCount && !getEmails)
        {
            queryDefinition.WithParameter("@offset", offset).WithParameter("@itemsPerPage", options.ItemsPerPage);
        }

        if (!string.IsNullOrEmpty(options.Search))
        {
            queryDefinition.WithParameter("@searchValue", options.Search);
        }

        if (!string.IsNullOrEmpty(options.ApplicationSearch))
        {
            queryDefinition.WithParameter("@applicationSearchValue", options.ApplicationSearch);
        }

        if (options.ShowingTodaysAppointments || options.SelectedDate != null)
        {
            queryDefinition.WithParameter("@today", options.SelectedDate != null ? options.SelectedDate?.ToString("yyyy-MM-dd") : DateTime.UtcNow.Date.ToString("yyyy-MM-dd"));
        }

        return queryDefinition;
    }

    public async Task<PermitApplication> GetLegacyApplication(string applicationId, CancellationToken cancellationToken)
    {
        return await _legacyContainer.ReadItemAsync<PermitApplication>(applicationId, new PartitionKey(applicationId), null, cancellationToken);
    }

    public async Task UpdateLegacyApplication(PermitApplication application, bool createApplication, CancellationToken cancellationToken)
    {
        if (createApplication)
        {
            await _container.CreateItemAsync(application, new PartitionKey(application.UserId), null, cancellationToken);
        }
        else
        {
            await _legacyContainer.UpsertItemAsync(application, new PartitionKey(application.Id.ToString()), null, cancellationToken);
        }
    }

    public async Task<bool> MatchUserInformation(string idNumber, string dateOfBirth, CancellationToken cancellationToken)
    {
        var queryString = "SELECT * FROM c WHERE c.Application.IdInfo.IdNumber = @idNumber AND c.Application.DOB.BirthDate = @dateOfBirth";

        var query = new QueryDefinition(queryString)
            .WithParameter("@idNumber", idNumber)
            .WithParameter("@dateOfBirth", dateOfBirth);

        using FeedIterator<PermitApplication> filteredFeed = _legacyContainer.GetItemQueryIterator<PermitApplication>(queryDefinition: query);

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<PermitApplication> response = await filteredFeed.ReadNextAsync(cancellationToken);

            return response.Resource.Any();
        }

        return false;
    }
}
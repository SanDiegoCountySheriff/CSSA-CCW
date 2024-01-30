using CCW.Common.Models;
using CCW.Schedule.Services.Contracts;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace CCW.Schedule.Services;

public class ApplicationCosmosDbService : IApplicationCosmosDbService
{
    private readonly Container _container;

    public ApplicationCosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
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

        if (application.PaymentHistory != null && application.PaymentHistory.Count > 0)
        {
            int paymentHistoryCount = application.PaymentHistory.Count;
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
}

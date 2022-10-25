using CCW.Schedule.Models;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;

namespace CCW.Schedule.Services;

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

    public async Task<AppointmentWindow> AddAsync(AppointmentWindow appointment)
    {
        try
        {
            AppointmentWindow createdItem = await _container.CreateItemAsync(appointment, new PartitionKey(appointment.Id));
            return createdItem;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task DeleteAsync(string appointmentId, string userId)
    {
        await _container.DeleteItemAsync<User>(appointmentId, new PartitionKey(userId));
    }

    public async Task UpdateAsync(AppointmentWindow appointment)
    {
        await _container.UpsertItemAsync(appointment, new PartitionKey(appointment.Id));
    }

    public async Task<AppointmentWindow> GetAsync(string appointmentId)
    {
        try
        {
            // Build query definition
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM appointments p WHERE p.id = @appointmentId"
            )
                .WithParameter("@appointmentId", appointmentId);


            // Query multiple items from container
            using FeedIterator<AppointmentWindow> filteredFeed = _container.GetItemQueryIterator<AppointmentWindow>(
                queryDefinition: parameterizedQuery
            );

            // Iterate query result pages
            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<AppointmentWindow> response = await filteredFeed.ReadNextAsync();

                return response.First();
            }

            return null!;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    public async Task<List<AppointmentWindow>> GetAvailableTimesAsync()
    {
        try
        {
            List<AppointmentWindow> availableTimes = new List<AppointmentWindow>();

            QueryDefinition queryDefinition = new QueryDefinition(
                    "SELECT * FROM appointments a where a.applicantId = null");
            using (FeedIterator<AppointmentWindow> feedIterator = _container.GetItemQueryIterator<AppointmentWindow>(
                       queryDefinition,
                       null))
            {
                while (feedIterator.HasMoreResults)
                {
                    foreach (var item in await feedIterator.ReadNextAsync())
                    {
                        Console.WriteLine(item);
                        availableTimes.Add(item);
                    }
                }
            }

            return availableTimes;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
}

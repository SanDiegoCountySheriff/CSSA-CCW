using CCW.Schedule.Entities;
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


    public async Task AddAvailableTimesAsync(List<AppointmentWindow> appointments)
    {
        List<Task> concurrentTasks = new List<Task>();

        foreach (AppointmentWindow appointment in appointments)
        {
            concurrentTasks.Add(
                _container.CreateItemAsync(appointment, new PartitionKey(appointment.Id.ToString()))
            );
        }

        await Task.WhenAll(concurrentTasks);
    }

    public async Task<AppointmentWindow> AddAsync(AppointmentWindow appointment)
    {
        AppointmentWindow createdItem = await _container.CreateItemAsync(appointment, new PartitionKey(appointment.Id.ToString()));
        return createdItem;
    }

    public async Task DeleteAsync(string appointmentId)
    {
        await _container.DeleteItemAsync<AppointmentWindow>(appointmentId, new PartitionKey(appointmentId));
    }

    public async Task UpdateAsync(AppointmentWindow appointment)
    {
        await _container.UpsertItemAsync(appointment, new PartitionKey(appointment.Id.ToString()));
    }

    public async Task<AppointmentWindow> GetAsync(string applicationId)
    {
        try
        {
            // Build query definition
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM appointments p WHERE p.applicationId = @applicationId"
            )
                .WithParameter("@applicationId", applicationId);


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
                    "SELECT * FROM appointments a where a.applicationId = null");
            using (FeedIterator<AppointmentWindow> feedIterator = _container.GetItemQueryIterator<AppointmentWindow>(
                       queryDefinition,
                       null))
            {
                while (feedIterator.HasMoreResults)
                {
                    foreach (var item in await feedIterator.ReadNextAsync())
                    {
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

    public async Task<List<AppointmentWindow>> GetAllBookedAppointmentsAsync()
    {
        try
        {
            List<AppointmentWindow> availableTimes = new List<AppointmentWindow>();

            QueryDefinition queryDefinition = new QueryDefinition(
                "SELECT * FROM appointments a where a.applicationId != null and a.applicationId != ''");

            using (FeedIterator<AppointmentWindow> feedIterator = _container.GetItemQueryIterator<AppointmentWindow>(
                       queryDefinition,
                       null))
            {
                while (feedIterator.HasMoreResults)
                {
                    foreach (var item in await feedIterator.ReadNextAsync())
                    {
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

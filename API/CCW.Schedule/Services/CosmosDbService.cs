using CCW.Schedule.Entities;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;


namespace CCW.Schedule.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;
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

    public async Task AddAvailableTimesAsync(List<AppointmentWindow> appointments, CancellationToken cancellationToken)
    {
        try
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
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to upload new available appointments: {cex.Message}");
            throw new Exception("An error occur while trying to upload new available appointments.");
        }
    }

    public async Task<AppointmentWindow> AddAsync(AppointmentWindow appointment, CancellationToken cancellationToken)
    {
        try
        {
            AppointmentWindow createdItem = await _container.CreateItemAsync(appointment, new PartitionKey(appointment.Id.ToString()));
            return createdItem;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to create new appointment: {appointment.Id}, {cex.Message}");
            throw new Exception("An error occur while trying to create new appointment.");
        }
    }

    public async Task UpdateAsync(AppointmentWindow appointment, CancellationToken cancellationToken)
    {
        try
        {
            await _container.UpsertItemAsync(appointment, new PartitionKey(appointment.Id.ToString()));
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to update appointment: {appointment.Id}, {cex.Message}");
            throw new Exception("An error occur while trying to update appointment.");
        }
    }

    public async Task<AppointmentWindow> GetAsync(string applicationId, CancellationToken cancellationToken)
    {
        try
        {
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM appointments p WHERE p.applicationId = @applicationId"
            )
                .WithParameter("@applicationId", applicationId);

            using FeedIterator<AppointmentWindow> filteredFeed = _container.GetItemQueryIterator<AppointmentWindow>(
                queryDefinition: parameterizedQuery
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<AppointmentWindow> response = await filteredFeed.ReadNextAsync();

                return response.First();
            }

            return null!;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve appointment for application: {applicationId}, {cex.Message}");
            throw new Exception("An error occur while trying to retrieve appointment.");
        }
    }

    public async Task<List<AppointmentWindow>> GetAvailableTimesAsync(CancellationToken cancellationToken)
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
                    foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
                    {
                        availableTimes.Add(item);
                    }
                }
            }

            return availableTimes;
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve available appointments: {cex.Message}");
            throw new Exception("An error occur while trying to retrieve available appointments");
        }
    }

    public async Task<List<AppointmentWindow>> GetAllBookedAppointmentsAsync(CancellationToken cancellationToken)
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
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve booked appointments: {cex.Message}");
            throw new Exception("An error occur while trying to retrieve booked appointments.");
        }
    }

    public async Task DeleteAsync(string appointmentId, CancellationToken cancellationToken)
    {
        try
        {
            await _container.DeleteItemAsync<AppointmentWindow>(appointmentId, new PartitionKey(appointmentId));
        }
        catch (CosmosException cex)
        {
            _logger.LogWarning($"An error occur while trying to retrieve delete appointment: {appointmentId}, {cex.Message}");
            throw new Exception("An error occur while trying to retrieve delete appointment.");
        }
    }
}
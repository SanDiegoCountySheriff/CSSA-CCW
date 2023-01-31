using CCW.Schedule.Entities;
using Microsoft.Azure.Cosmos;
using System.Globalization;
using Container = Microsoft.Azure.Cosmos.Container;


namespace CCW.Schedule.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public string SessionToken { get; set; }

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public async Task AddAvailableTimesAsync(List<AppointmentWindow> appointments, CancellationToken cancellationToken)
    {
        List<Task> concurrentTasks = new List<Task>();

        foreach (AppointmentWindow appointment in appointments)
        {
            concurrentTasks.Add(
                _container.CreateItemAsync(appointment, new PartitionKey(appointment.Id.ToString()), cancellationToken: cancellationToken)
            );
        }

        await Task.WhenAll(concurrentTasks);
    }

    public async Task AddDeleteTimeSlotsAsync(List<AppointmentWindow> appointments, bool isCreateAction, CancellationToken cancellationToken)
    {
        var existingAppointments = await GetExistingAppointments(
            appointments.Select(x => x.Start).FirstOrDefault(),
            appointments.Select(x => x.End).FirstOrDefault(),
            cancellationToken);

        //  var existingAppointments = new List<AppointmentWindow>();

        int numOfIncomingApptSlots = appointments.Count();
        int numOfExistingApptSlots = existingAppointments.Count();
        int numOfBookedAppts = existingAppointments.Count(x => x.ApplicationId != null);
        List<AppointmentWindow> availExistingSlots = existingAppointments.Where(x => x.ApplicationId == null).ToList();

        List<Task> concurrentTasks = new List<Task>();

        //create
        if ((numOfIncomingApptSlots - numOfExistingApptSlots) > 0 && isCreateAction)
        {
            //add 
            var addExtraSlots = numOfIncomingApptSlots - numOfExistingApptSlots;
            for (int i = 0; i < addExtraSlots; i++)
            {
                concurrentTasks.Add(
                    _container.CreateItemAsync(appointments[i], new PartitionKey(appointments[i].Id.ToString()),
                        cancellationToken: cancellationToken)
                );
            }
        }

        if ((numOfIncomingApptSlots - numOfExistingApptSlots) < 0 && (numOfIncomingApptSlots > numOfBookedAppts) && isCreateAction)
        {
           //delete
           var deleteExtraSlots = numOfExistingApptSlots - numOfIncomingApptSlots;

           for (int i = 0; i < deleteExtraSlots; i++)
           {
               concurrentTasks.Add(
                   _container.DeleteItemAsync<AppointmentWindow>(availExistingSlots[i].Id.ToString(),
                       new PartitionKey(availExistingSlots[i].Id.ToString()),
                       cancellationToken: cancellationToken)
                   );
           }
        }

        if ((numOfIncomingApptSlots - numOfExistingApptSlots) < 0 && (numOfIncomingApptSlots < numOfBookedAppts) && isCreateAction)
        {
            //delete
            var deleteSlots = numOfExistingApptSlots - numOfBookedAppts;
            for (int i = 0; i < deleteSlots; i++)
            {
                concurrentTasks.Add(
                    _container.DeleteItemAsync<AppointmentWindow>(availExistingSlots[i].Id.ToString(), new PartitionKey(availExistingSlots[i].Id.ToString()),
                    cancellationToken: cancellationToken)
                    );
            }
        }

        //delete
        if (!isCreateAction)
        {
            //delete
            var availToDeleteSlots = numOfExistingApptSlots - numOfBookedAppts;

            if (numOfIncomingApptSlots > availToDeleteSlots)
            {
                for (int i = 0; i < availToDeleteSlots; i++)
                {
                    concurrentTasks.Add(
                        _container.DeleteItemAsync<AppointmentWindow>(availExistingSlots[i].Id.ToString(),
                            new PartitionKey(availExistingSlots[i].Id.ToString()),
                            cancellationToken: cancellationToken)
                    );
                }
            }
            else
            {
                for (int i = 0; i < numOfIncomingApptSlots; i++)
                {
                    concurrentTasks.Add(
                        _container.DeleteItemAsync<AppointmentWindow>(availExistingSlots[i].Id.ToString(),
                            new PartitionKey(availExistingSlots[i].Id.ToString()),
                            cancellationToken: cancellationToken)
                    );
                }
            }
        }

        await Task.WhenAll(concurrentTasks);
    }

    private async Task<List<AppointmentWindow>> GetExistingAppointments(DateTime startDateTime, DateTime endDateTime, CancellationToken cancellationToken)
    {
        List<AppointmentWindow> existingAppointments = new List<AppointmentWindow>();

        var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM appointments a where a['start'] = @startTime and a['end'] = @endTime"
            )
            .WithParameter("@startTime", startDateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
            .WithParameter("@endTime", endDateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));

        using FeedIterator<AppointmentWindow> feedIterator = _container.GetItemQueryIterator<AppointmentWindow>(
            queryDefinition: parameterizedQuery
        );

        while (feedIterator.HasMoreResults)
        {
            foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
            {
                existingAppointments.Add(item);
            }
        }

        return existingAppointments;
    }

    public async Task<AppointmentWindow> AddAsync(AppointmentWindow appointment, CancellationToken cancellationToken)
    {
        AppointmentWindow createdItem = await _container.CreateItemAsync(appointment, new PartitionKey(appointment.Id.ToString()));
        return createdItem;
    }

    public async Task UpdateAsync(AppointmentWindow appointment, CancellationToken cancellationToken)
    {
        await _container.UpsertItemAsync(appointment, new PartitionKey(appointment.Id.ToString()));
    }

    public async Task<AppointmentWindow> GetAsync(string applicationId, CancellationToken cancellationToken)
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

    public async Task<List<AppointmentWindow>> GetAvailableTimesAsync(CancellationToken cancellationToken)
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

    public async Task<List<AppointmentWindow>> GetAllBookedAppointmentsAsync(CancellationToken cancellationToken)
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

    public async Task<AppointmentWindow> GetAppointmentByIdAsync(string appointmentId, CancellationToken cancellationToken)
    {
        var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM appointments p WHERE p.id = @appointmentId"
            )
            .WithParameter("@appointmentId", appointmentId);

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

    public async Task DeleteAsync(string appointmentId, CancellationToken cancellationToken)
    {
        await _container.DeleteItemAsync<AppointmentWindow>(appointmentId, new PartitionKey(appointmentId), cancellationToken:cancellationToken);
    }
}
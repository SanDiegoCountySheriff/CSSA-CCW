using CCW.Common.Models;
using CCW.Schedule.Entities;
using Microsoft.Azure.Cosmos;
using PublicHoliday;
using System.Globalization;
using Container = Microsoft.Azure.Cosmos.Container;


namespace CCW.Schedule.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;
    private readonly Container _holidayContainer;

    public string SessionToken { get; set; }

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName,
        string holidayContainerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
        _holidayContainer = cosmosDbClient.GetContainer(databaseName, holidayContainerName);
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

    public async Task<List<AppointmentWindow>> GetExistingAppointments(DateTime startDateTime, DateTime endDateTime, CancellationToken cancellationToken)
    {
        List<AppointmentWindow> existingAppointments = new List<AppointmentWindow>();

        var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM appointments a where a['start'] = @startTime and a['end'] = @endTime"
            )
            .WithParameter("@startTime", startDateTime)
            .WithParameter("@endTime", endDateTime);

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

    public async Task AddOrganizationalHoliday(OrganizationHolidays organizationalHolidays, CancellationToken cancellationToken)
    {
        await _holidayContainer.UpsertItemAsync(organizationalHolidays, new PartitionKey(organizationalHolidays.Id.ToString()));
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

    public async Task<List<AppointmentWindow>> ResetApplicantAppointmentsAsync(string applicationId, CancellationToken cancellationToken)
    {
        var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM appointments p WHERE p.applicationId = @applicationId"
            ).WithParameter("@applicationId", applicationId);

        using FeedIterator<AppointmentWindow> filteredFeed =
            _container.GetItemQueryIterator<AppointmentWindow>(queryDefinition: parameterizedQuery);

        List<AppointmentWindow> appointments = new List<AppointmentWindow>();
        while (filteredFeed.HasMoreResults)
        {
            var nextResult = await filteredFeed.ReadNextAsync();
            appointments.AddRange(nextResult.ToList());

            List<Task> tasks = new List<Task>(appointments.Count);

            foreach (var appointment in appointments)
            {
                appointment.ApplicationId = null;
                appointment.UserId = null;
                appointment.Status = AppointmentStatus.Available;
                appointment.Name = null;
                appointment.Permit = null;
                appointment.Payment = null;

                tasks.Add(Update(appointment));
            }

            await Task.WhenAll(tasks);
        }


        return appointments;
    }

    private async Task Update(AppointmentWindow appointment)
    {
        await _container.ReplaceItemAsync(
            appointment,
            appointment.Id.ToString(),
            new PartitionKey(appointment.Id.ToString()),
            new ItemRequestOptions { IfMatchEtag = appointment._etag }
        );
    }

    public async Task<List<AppointmentWindow>> GetAvailableSlotByDateTime(DateTime startTime, CancellationToken cancellationToken)
    {
        List<AppointmentWindow> availableTimes = new List<AppointmentWindow>();

        string query = $@"SELECT * 
                        FROM appointments a
                        WHERE a.start = '{startTime.ToString("yyyy-MM-ddTHH:mm:ssZ")}' 
                            AND a.applicationId = null 
                            AND a.isManuallyCreated = false
                        ORDER BY a.start
                        OFFSET 0 LIMIT 1";

        QueryDefinition queryDefinition = new QueryDefinition(query);

        using (FeedIterator<AppointmentWindow> feedIterator = _container.GetItemQueryIterator<AppointmentWindow>(
                   queryDefinition,
                   null))
        {
            while (feedIterator.HasMoreResults)
            {
                foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
                {
                    item.IsManuallyCreated = false;

                    availableTimes.Add(item);
                }
            }
        }

        return availableTimes.OrderBy(i => i.Start).ToList();
    }

    public async Task<List<AppointmentWindow>> GetAvailableTimesAsync(CancellationToken cancellationToken)
    {
        List<AppointmentWindow> availableTimes = new List<AppointmentWindow>();

        string query = @"SELECT a.start, a[""end""]
                FROM a
                WHERE a.applicationId = null AND a.isManuallyCreated = false
                GROUP BY a.start, a[""end""]";

        QueryDefinition queryDefinition = new QueryDefinition(query);

        using (FeedIterator<AppointmentWindow> feedIterator = _container.GetItemQueryIterator<AppointmentWindow>(
                   queryDefinition,
                   null))
        {
            while (feedIterator.HasMoreResults)
            {
                foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
                {
                    item.IsManuallyCreated = false;

                    availableTimes.Add(item);
                }
            }
        }

        return availableTimes.OrderBy(i => i.Start).ToList();
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

    public async Task<AppointmentWindow> GetAppointmentByUserIdAsync(string userId, CancellationToken cancellation)
    {
        var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM appointments p WHERE p.userId = @userId"
            )
            .WithParameter("@userId", userId);

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
        await _container.DeleteItemAsync<AppointmentWindow>(appointmentId, new PartitionKey(appointmentId), cancellationToken: cancellationToken);
    }

    public async Task<int> DeleteAllAppointmentsByDate(DateTime date, CancellationToken cancellationToken)
    {

        DateTime endDate = date.AddDays(1).AddTicks(1);

        var parameterizedQuery = new QueryDefinition(
        query: "SELECT * FROM c WHERE c.start >= @startDate AND c.start <= @endDate AND (NOT IS_DEFINED(c.applicationId) OR c.applicationId = null)"
        )
        .WithParameter("@startDate", date)
        .WithParameter("@endDate", endDate);

        var documentIds = new List<Guid>();
        var resultSetIterator = _container.GetItemQueryIterator<AppointmentWindow>(parameterizedQuery);

        while (resultSetIterator.HasMoreResults)
        {
            var response = await resultSetIterator.ReadNextAsync();

            foreach (var document in response)
            {
                documentIds.Add(document.Id);
            }
        }

        var concurrentTasks = new List<Task>();
        int deletedCount = 0;

        foreach (Guid id in documentIds)
        {
            concurrentTasks.Add(_container.DeleteItemAsync<AppointmentWindow>(id.ToString(), new PartitionKey(id.ToString()), null, cancellationToken));
            deletedCount++;
        }

        await Task.WhenAll(concurrentTasks);

        return deletedCount;
    }

    public async Task<int> DeleteAppointmentsByTimeSlot(DateTime date, CancellationToken cancellationToken)
    {

        var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM c WHERE c.start = @date AND (NOT IS_DEFINED(c.applicationId) OR c.applicationId = null)"
            )
            .WithParameter("@date", date);

        var documentIds = new List<Guid>();
        var resultSetIterator = _container.GetItemQueryIterator<AppointmentWindow>(parameterizedQuery);

        while (resultSetIterator.HasMoreResults)
        {
            var response = await resultSetIterator.ReadNextAsync();

            foreach (var document in response)
            {
                documentIds.Add(document.Id);
            }
        }

        var concurrentTasks = new List<Task>();
        int deletedCount = 0;

        foreach (Guid id in documentIds)
        {
            concurrentTasks.Add(_container.DeleteItemAsync<AppointmentWindow>(id.ToString(), new PartitionKey(id.ToString()), null, cancellationToken));
            deletedCount++;
        }

        await Task.WhenAll(concurrentTasks);

        return deletedCount;
    }

    public async Task<(int, int)> CreateAppointmentsFromAppointmentManagementTemplate(AppointmentManagement appointmentManagement, CancellationToken cancellationToken)
    {
        var concurrentTasks = new List<Task>();
        var count = 0;
        var holidayCount = 0;
        var query = _container.GetItemQueryIterator<AppointmentWindow>("SELECT TOP 1 c.start FROM c ORDER BY c.start DESC");
        var nextDay = DateTime.UtcNow;

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();

            foreach (var item in response)
            {
                DateTime startDate = item.Start;
                nextDay = startDate.AddDays(1);
            }
        }

        for (int i = 0; i < appointmentManagement.NumberOfWeeksToCreate; i++)
        {
            foreach (var dayOfTheWeek in appointmentManagement.DaysOfTheWeek)
            {
                DateTime currentDate = nextDay.AddDays(i * 7);

                while (currentDate < appointmentManagement.StartDate)
                {
                    currentDate = currentDate.AddDays(1);
                }

                while (currentDate.DayOfWeek != (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfTheWeek))
                {
                    currentDate = currentDate.AddDays(1);
                }

                var observedHolidays = await GetObservedOrganizationalHolidaysByYear(currentDate.Year);

                if (observedHolidays.Contains(currentDate.Date))
                {
                    holidayCount += 1;
                    continue;
                }

                var startTime = appointmentManagement.FirstAppointmentStartTime;
                var endTime = appointmentManagement.FirstAppointmentStartTime.Add(TimeSpan.FromMinutes(appointmentManagement.AppointmentLength));
                var lastAppointmentStartTime = appointmentManagement.LastAppointmentStartTime;

                if (lastAppointmentStartTime < startTime)
                {
                    lastAppointmentStartTime = lastAppointmentStartTime.Add(TimeSpan.FromDays(1));
                }

                while (startTime <= lastAppointmentStartTime)
                {
                    if (appointmentManagement.BreakStartTime != null && WillAppointmentFallInBreakTime(appointmentManagement, startTime))
                    {
                        startTime = startTime.Add(new TimeSpan(0, appointmentManagement.AppointmentLength, 0));
                        endTime = endTime.Add(new TimeSpan(0, appointmentManagement.AppointmentLength, 0));
                        continue;
                    }

                    for (var j = 0; j < appointmentManagement.NumberOfSlotsPerAppointment; j++)
                    {
                        var appointment = new AppointmentWindow()
                        {
                            Id = Guid.NewGuid(),
                            Start = currentDate.Date + startTime,
                            End = currentDate.Date + endTime,
                        };

                        concurrentTasks.Add(_container.CreateItemAsync(appointment, new PartitionKey(appointment.Id.ToString()), cancellationToken: cancellationToken));

                        count += 1;
                    }

                    startTime = startTime.Add(new TimeSpan(0, appointmentManagement.AppointmentLength, 0));
                    endTime = endTime.Add(new TimeSpan(0, appointmentManagement.AppointmentLength, 0));
                }
            }
        }

        await Task.WhenAll(concurrentTasks);

        return (count, holidayCount);
    }

    private bool WillAppointmentFallInBreakTime(AppointmentManagement appointmentManagement, TimeSpan startTime)
    {
        TimeSpan breakEnd = appointmentManagement.BreakStartTime.Value.Add(TimeSpan.FromMinutes(appointmentManagement.BreakLength ?? appointmentManagement.AppointmentLength));

        return startTime >= appointmentManagement.BreakStartTime && startTime < breakEnd;
    }

    public async Task<int> GetNumberOfNewAppointments(int numberOfDays, CancellationToken cancellationToken)
    {
        var day = DateTime.UtcNow.AddDays(numberOfDays * -1).ToString("o", CultureInfo.InvariantCulture);

        var parameterizedQuery = new QueryDefinition(
            query: "SELECT VALUE Count(c) FROM c WHERE c.appointmentCreatedDate > @day"
            )
            .WithParameter("@day", day);

        using FeedIterator<int> filteredFeed = _container.GetItemQueryIterator<int>(
            queryDefinition: parameterizedQuery, requestOptions: new QueryRequestOptions { MaxItemCount = 1 }
            );

        if (filteredFeed.HasMoreResults)
        {
            var response = await filteredFeed.ReadNextAsync();

            return response.FirstOrDefault();
        }

        return 0;
    }

    public async Task<string> GetNextAvailableAppointment()
    {
        var day = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);

        var parameterizedQuery = new QueryDefinition(
            query: "SELECT TOP 1 VALUE c.start FROM c WHERE c.start > @day AND c.status = 0"
            )
            .WithParameter("@day", day);

        using FeedIterator<string> filteredFeed = _container.GetItemQueryIterator<string>(
            queryDefinition: parameterizedQuery, requestOptions: new QueryRequestOptions { MaxItemCount = 1 }
            );

        if (filteredFeed.HasMoreResults)
        {
            var response = await filteredFeed.ReadNextAsync();

            return response.FirstOrDefault();
        }

        return string.Empty;
    }

    public async Task<OrganizationHolidays> GetOrganizationalHolidays()
    {
        var parameterizedQuery = new QueryDefinition("SELECT * FROM c");

        using FeedIterator<OrganizationHolidays> filteredFeed = _holidayContainer.GetItemQueryIterator<OrganizationHolidays>(
            queryDefinition: parameterizedQuery
        );

        if (filteredFeed.HasMoreResults)
        {
            FeedResponse<OrganizationHolidays> response = await filteredFeed.ReadNextAsync();

            return response.FirstOrDefault();
        }

        return null!;
    }

    private DateTime FixWeekendSaturdayBeforeSundayAfter(DateTime holiday)
    {
        if (holiday.DayOfWeek == DayOfWeek.Sunday)
        {
            holiday = holiday.AddDays(1.0);
        }
        else if (holiday.DayOfWeek == DayOfWeek.Saturday)
        {
            holiday = holiday.AddDays(-1.0);
        }

        return holiday;
    }

    private async Task<List<DateTime>> GetObservedOrganizationalHolidaysByYear(int year)
    {
        List<DateTime> observedHolidays = new();
        var organizationHolidays = await GetOrganizationalHolidays();
        List<Holiday> holidays = new USAPublicHoliday().PublicHolidaysInformation(year).ToList();

        if (organizationHolidays != null)
        {
            foreach (var holiday in holidays)
            {
                foreach (var organizationHoliday in organizationHolidays.Holidays)
                {
                    if (holiday.GetName() == organizationHoliday.Name)
                    {
                        observedHolidays.Add(holiday.ObservedDate.Date);
                        continue;
                    }

                    if (organizationHoliday.Name == "CesarChavez")
                    {
                        observedHolidays.Add(FixWeekendSaturdayBeforeSundayAfter(new DateTime(year, organizationHoliday.Month, organizationHoliday.Day).Date));
                        continue;
                    }
                }
            }
        }

        return observedHolidays;
    }
}
using Moq;
using System.Net;
using Microsoft.Azure.Cosmos;
using CCW.Schedule.Entities;
using CCW.Schedule.Services;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace CCW.Schedule.Tests;

internal class CosmosDbServiceTests
{
    protected string _databaseNameMock { get; }
    protected string _containerNameMock { get; }
    protected Mock<CosmosClient> _cosmosClientMock { get; }
    protected Mock<ILogger<CosmosDbService>> _loggerMock { get; }


    public CosmosDbServiceTests()
    {
        _databaseNameMock = "schedule-database";
        _containerNameMock = "schedule";
        _cosmosClientMock = new Mock<CosmosClient>();
        _loggerMock = new Mock<ILogger<CosmosDbService>>();
    }

    [AutoMoqData]
    [Test]
    public async Task AddAvailableTimesAsync_Should_Add_AllAvailableAppointments()
    {
        // Arrange
        List<AppointmentWindow> appointments = new List<AppointmentWindow>();
        AppointmentWindow appt = new AppointmentWindow
        {
            Id = Guid.NewGuid(),
            ApplicationId = null,
            End = new DateTime(),
            Start = new DateTime().AddHours(1),
            Status = null,
            Name = null,
            Permit = null,
            Payment = null,
            IsManuallyCreated = false,
        };
        appointments.Add(appt);

        var responseMock = new Mock<ItemResponse<AppointmentWindow>>();
        responseMock.Setup(x => x.Resource).Returns(appt);

        var container = new Mock<Container>();
        container.Setup(x => x.CreateItemAsync(
                It.IsAny<AppointmentWindow>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        await sut.AddAvailableTimesAsync(appointments, default);

        // Assert
        container.Verify();
    }

    [AutoMoqData]
    [Test]
    public async Task AddAvailableTimesAsync_Should_Throw_When_Error(List<AppointmentWindow> appointments)
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(_ => _.CreateItemAsync(
            It.IsAny<AppointmentWindow>(),
            It.IsAny<PartitionKey>(),
            It.IsAny<ItemRequestOptions>(),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.AddAvailableTimesAsync(appointments, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to upload new available appointments.");
    }

    [AutoMoqData]
    [Test]
    public async Task AddAsync_Should_Add_AppointmentWindow(AppointmentWindow appointment)
    {
        // Arrange
        var responseMock = new Mock<ItemResponse<AppointmentWindow>>();
        responseMock.Setup(x => x.Resource).Returns(appointment);

        var container = new Mock<Container>();
        container.Setup(x => x.CreateItemAsync(
            It.IsAny<AppointmentWindow>(),
            It.IsAny<PartitionKey>(),
            It.IsAny<ItemRequestOptions>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.AddAsync(appointment, default);

        // Assert
        result.Should().Be(appointment);
    }

    [AutoMoqData]
    [Test]
    public async Task AddAsync_Should_Throw_When_Error(AppointmentWindow appointment)
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.CreateItemAsync(
                It.IsAny<AppointmentWindow>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.AddAsync(appointment, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to create new appointment.");
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateAsync_ShouldAdd_AppointmentWindow(AppointmentWindow appointment)
    {
        // Arrange
        var responseMock = new Mock<ItemResponse<AppointmentWindow>>();
        responseMock.Setup(x => x.Resource).Returns(appointment);

        var container = new Mock<Container>();
        container.Setup(x => x.UpsertItemAsync(
                It.IsAny<AppointmentWindow>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        await sut.UpdateAsync(appointment, default);

        // Assert
        container.Verify(_ => _.UpsertItemAsync(
            appointment,
            It.IsAny<PartitionKey>(),
            It.IsAny<ItemRequestOptions>(),
            default), Times.Once);
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateAsync_Should_Throw_When_Error(AppointmentWindow appointment)
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.UpsertItemAsync(
                It.IsAny<AppointmentWindow>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UpdateAsync(appointment, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to update appointment.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetAsync_ShouldReturn_AppointmentWindow(AppointmentWindow appointment)
    {
        // Arrange
        var appointments = new List<AppointmentWindow> { appointment };

        var feedResponseMock = new Mock<FeedResponse<AppointmentWindow>>();
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(appointments.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<AppointmentWindow>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AppointmentWindow>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.GetAsync(appointment.ApplicationId, default);

        // Assert
        result.Should().Be(appointment);
    }

    [AutoMoqData]
    [Test]
    public async Task GetAsync_ShouldReturn_Null_When_AppointmentNotFound(AppointmentWindow appointment)
    {
        // Arrange
        var appointments = new List<AppointmentWindow>();

        var feedResponseMock = new Mock<FeedResponse<AppointmentWindow>>();
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(appointments.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<AppointmentWindow>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(false);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AppointmentWindow>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.GetAsync(appointment.ApplicationId, default);

        // Assert
        result.Should().Be(null);
    }

    [AutoMoqData]
    [Test]
    public async Task GetAsync_Should_Throw_When_Error(string applicationId)
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AppointmentWindow>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAsync(applicationId, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve appointment.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetAvailableTimesAsync_ShouldReturn_A_ListOfAvailable_AppointmentWindow(AppointmentWindow appointment)
    {
        // Arrange
        appointment.ApplicationId = null;
        var appointments = new List<AppointmentWindow> { appointment };

        var feedResponseMock = new Mock<FeedResponse<AppointmentWindow>>();
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(appointments.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<AppointmentWindow>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AppointmentWindow>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.GetAvailableTimesAsync(default);

        // Assert
        result.Count.Should().Be(1);
        result.Should().Contain(appointment);
    }

    [Test]
    public async Task GetAvailableTimesAsync_Should_Throw_When_Error()
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AppointmentWindow>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAvailableTimesAsync(default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve available appointments");
    }

    [AutoMoqData]
    [Test]
    public async Task GetAllBookedAppointmentsAsync_ShouldReturn_A_ListOfBooked_AppointmentWindow(AppointmentWindow appointment)
    {
        // Arrange
        var appointments = new List<AppointmentWindow> { appointment };

        var feedResponseMock = new Mock<FeedResponse<AppointmentWindow>>();
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(appointments.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<AppointmentWindow>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AppointmentWindow>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.GetAllBookedAppointmentsAsync(default);

        // Assert
        result.Count.Should().Be(1);
        result.Should().Contain(appointment);
    }

    [Test]
    public async Task GetAllBookedAppointmentsAsync_Should_Throw_When_Error()
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AppointmentWindow>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAllBookedAppointmentsAsync(default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve booked appointments.");
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteAsync_Should_AppointmentWindow(string appointmentId)
    {
        // Arrange
        var responseMock = new Mock<ItemResponse<AppointmentWindow>>();
        responseMock.Setup(x => x.Resource);

        var container = new Mock<Container>();
        container.Setup(x => x.DeleteItemAsync<AppointmentWindow>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        await sut.DeleteAsync(appointmentId, default);

        // Assert
        container.Verify(_ => _.DeleteItemAsync<AppointmentWindow>(
            appointmentId,
            It.IsAny<PartitionKey>(),
            It.IsAny<ItemRequestOptions>(),
            default), Times.Once);
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteAsync_Should_Throw_When_Error(string appointmentId)
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.DeleteItemAsync<AppointmentWindow>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DeleteAsync(appointmentId, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve delete appointment.");
    }
}
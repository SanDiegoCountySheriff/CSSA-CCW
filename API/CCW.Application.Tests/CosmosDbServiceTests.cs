using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;
using CCW.Application.Services;
using CCW.Application.Entities;
using System.Net;
using FluentAssertions;

namespace CCW.Application.Tests;

internal class CosmosDbServiceTests
{
    protected string _databaseNameMock { get; }
    protected string _containerNameMock { get; }
    protected Mock<CosmosClient> _cosmosClientMock { get; }
    protected Mock<ILogger<CosmosDbService>> _loggerMock { get; }


    public CosmosDbServiceTests()
    {
        _databaseNameMock = "user-database";
        _containerNameMock = "users";
        _cosmosClientMock = new Mock<CosmosClient>();
        _loggerMock = new Mock<ILogger<CosmosDbService>>();
    }

    [AutoMoqData]
    [Test]
    public async Task AddAsync_Should_Return_PermitApplication(
        PermitApplication application
    )
    {
        // Arrange
        var responseMock = new Mock<ItemResponse<PermitApplication>>();
        responseMock.Setup(x => x.Resource).Returns(application);

        var container = new Mock<Container>();
        container.Setup(x => x.CreateItemAsync(
                It.IsAny<PermitApplication>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.AddAsync(application, default);

        // Assert
        result.Should().Be(application);
    }

    [AutoMoqData]
    [Test]
    public async Task AddAsync_Should_Throw_When_Error(
        PermitApplication application
    )
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.CreateItemAsync(
                It.IsAny<PermitApplication>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.AddAsync(application, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to add new permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteItemAsync_Should_Delete_PermitApplication(
        string applicationId,
        string userId
    )
    {
        // Arrange
        var mockItemResponse = new Mock<ItemResponse<PermitApplication>>();
        mockItemResponse.Setup(x => x.StatusCode)
            .Returns(HttpStatusCode.OK);

        var container = new Mock<Container>();
        container.Setup(x => x.DeleteItemAsync<PermitApplication>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockItemResponse.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        await sut.DeleteAsync(applicationId, userId, cancellationToken: default);

        // Assert
        container.Verify(mock => mock.DeleteItemAsync<PermitApplication>(applicationId,
            new PartitionKey(applicationId), null,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteItemAsync_Should_Throw_When_Error(
        string applicationId,
        string userId
    )
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.DeleteItemAsync<PermitApplication>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DeleteAsync(applicationId, userId, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to delete permit application.");
    }


    [AutoMoqData]
    [Test]
    public async Task GetAsync_Should_Return_PermitApplication(
        PermitApplication application,
        string userEmailOrOrderId,
        bool isOrderId,
        bool isComplete
    )
    {
        // Arrange
        var applications = new List<PermitApplication> { application };

        var feedResponseMock = new Mock<FeedResponse<PermitApplication>>();
        feedResponseMock.SetupGet(p => p.Resource).Returns(applications);
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(applications.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<PermitApplication>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<PermitApplication>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.GetAsync(userEmailOrOrderId, isOrderId, isComplete, default);

        // Assert
        result.Id.Should().Be(application.Id);
        result.Should().BeOfType<PermitApplication>();
        //result.History.Should().Contain(application.History);
    }

    [AutoMoqData]
    [Test]
    public async Task GetAsync_Should_Throw_When_Error(
        string userEmailOrOrderId,
        bool isOrderId,
        bool isComplete
    )
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<PermitApplication>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAsync(userEmailOrOrderId, isOrderId, isComplete, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task ListAsync_Should_Return_AList_Of_PermitApplication(
       PermitApplication application,
       int startIndex,
       int count
   )
    {
        // Arrange
        var applications = new List<PermitApplication> { application };

        var feedResponseMock = new Mock<FeedResponse<PermitApplication>>();
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(applications.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<PermitApplication>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<PermitApplication>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.ListAsync(startIndex, count, default);

        // Assert
        result.Count().Should().Be(1);
        result.Should().BeOfType<List<PermitApplication>>();
        result.FirstOrDefault().Should().Be(application);
    }

    [AutoMoqData]
    [Test]
    public async Task ListAsync_Should_Throw_When_Error(
        int startIndex,
        int count
    )
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<PermitApplication>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.ListAsync(startIndex, count, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve all permit applications.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetMultipleAsync_Should_Return_AList_Of_PermitApplication(
      PermitApplication application
    )
    {
        // Arrange
        var applications = new List<PermitApplication> { application };

        var feedResponseMock = new Mock<FeedResponse<PermitApplication>>();
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(applications.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<PermitApplication>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<PermitApplication>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.GetMultipleAsync(default);

        // Assert
        result.Count().Should().Be(1);
        result.Should().BeOfType<List<PermitApplication>>();
        result.FirstOrDefault().Should().Be(application);
    }

    [AutoMoqData]
    [Test]
    public async Task GetMultipleAsync_Should_Throw_When_Error()
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<PermitApplication>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetMultipleAsync(default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve all permit applications.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetAllApplicationsAsync_Should_Return_AList_Of_PermitApplication(
        SummarizedPermitApplication application
    )
    {
        // Arrange
        var applications = new List<SummarizedPermitApplication> { application };

        var feedResponseMock = new Mock<FeedResponse<SummarizedPermitApplication>>();
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(applications.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<SummarizedPermitApplication>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<SummarizedPermitApplication>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.GetAllApplicationsAsync(default);

        // Assert
        result.Count().Should().Be(1);
        result.Should().BeOfType<List<SummarizedPermitApplication>>();
        result.FirstOrDefault().Should().Be(application);
    }

    [AutoMoqData]
    [Test]
    public async Task GetAllApplicationsAsync_Should_Throw_When_Error()
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<SummarizedPermitApplication>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAllApplicationsAsync(default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve all summarized permit applications.");
    }


    [AutoMoqData]
    [Test]
    public async Task UpdateAsync_Should_Succesfully_Update_Application_And_History(
        PermitApplication application
    )
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.PatchItemAsync<PermitApplication>(
                application.Id.ToString(),
                new PartitionKey(application.Id.ToString()),
                new[]
                {
                    PatchOperation.Set("/Application", It.IsAny<PermitApplication>()),
                    PatchOperation.Add("/History/-", It.IsAny<History>())
                },
                null,
                It.IsAny<CancellationToken>()))
           .Returns(value: null!);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        await sut.UpdateAsync(application, default);

        // Assert
        container.Verify(mock => mock.PatchItemAsync<PermitApplication>(
            It.IsAny<string>(),
            It.IsAny<PartitionKey>(),
            It.Is<IReadOnlyList<PatchOperation>>(x =>
                x != null && x.Count() == 2 && (x.First().Path == "/Application" || x.Last().Path == "/History/-")
            ),
            null,
            default), Times.Once);
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateAsync_Should_Throw_When_Error(
        PermitApplication application
    )
    {
        // Arrange
        var responseMock = new Mock<ItemResponse<PermitApplication>>();

        var container = new Mock<Container>();
        container.Setup(x => x.PatchItemAsync<PermitApplication>(
                application.Id.ToString(),
                new PartitionKey(application.Id.ToString()),
                It.IsAny<PatchOperation[]>(),
                null,
                default))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UpdateAsync(application, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to update permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetApplicationHistoryAsync_Should_Return_AList_Of_PermitApplication_History(
        string applicationIdOrOrderId,
        bool isOrderId,
        HistoryResponse history
    )
    {
        // Arrange
        var historyList = new List<HistoryResponse> { history };
        var mockFeed = new Mock<FeedResponse<HistoryResponse>>();
        mockFeed.SetupGet(p => p.Resource).Returns(historyList);
        mockFeed.Setup(x => x.GetEnumerator()).Returns(historyList.GetEnumerator());

        var mockIterator = new Mock<FeedIterator<HistoryResponse>>();
        mockIterator.SetupSequence(p => p.HasMoreResults)
            .Returns(true)
            .Returns(false);
        mockIterator.Setup(p => p.ReadNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockFeed.Object);

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<HistoryResponse>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(mockIterator.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        // Act
        var result = await sut.GetApplicationHistoryAsync(applicationIdOrOrderId, default, isOrderId);

        // Assert
        result.Count().Should().BeGreaterOrEqualTo(1);
        result.Should().BeOfType<List<History>>();
        result.Should().Contain(history.History);
    }

    [AutoMoqData]
    [Test]
    public async Task GetApplicationHistoryAsync_Should_Throw_When_Error(
        string applicationIdOrOrderId,
        bool isOrderId
        )
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<HistoryResponse>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock, _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetApplicationHistoryAsync(applicationIdOrOrderId, default, isOrderId)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve permit application history.");
    }
}

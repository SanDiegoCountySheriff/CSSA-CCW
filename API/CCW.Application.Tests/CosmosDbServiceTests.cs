﻿using Microsoft.Azure.Cosmos;
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


    public CosmosDbServiceTests()
    {
        _databaseNameMock = "user-database";
        _containerNameMock = "users";
        _cosmosClientMock = new Mock<CosmosClient>();
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

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.AddAsync(application, default);

        // Assert
        result.Should().Be(application);
    }


    [AutoMoqData]
    [Test]
    public async Task GetLastApplicationAsync_Should_Return_PermitApplication(
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

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.GetLastApplicationAsync(userEmailOrOrderId, isOrderId, isComplete, default);

        // Assert
        result.Id.Should().Be(application.Id);
        result.Should().BeOfType<PermitApplication>();
    }


    [AutoMoqData]
    [Test]
    public async Task GetAllUserApplicationsAsync_Should_Return_PermitApplication(
        PermitApplication application,
        string userEmailOrOrderId
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

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.GetAllUserApplicationsAsync(userEmailOrOrderId, default);

        // Assert
        result.Select(x=>x.Id.Should().Be(application.Id));
        result.Should().BeOfType<List<PermitApplication>>();
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

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.GetApplicationHistoryAsync(applicationIdOrOrderId, default, isOrderId);

        // Assert
        result.Count().Should().BeGreaterOrEqualTo(1);
        result.Should().BeOfType<List<History>>();
        result.Should().Contain(history.History);
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

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.GetAllApplicationsAsync(default);

        // Assert
        result.Count().Should().Be(1);
        result.Should().BeOfType<List<SummarizedPermitApplication>>();
        result.FirstOrDefault().Should().Be(application);
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

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

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
}
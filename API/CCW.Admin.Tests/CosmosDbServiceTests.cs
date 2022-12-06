using System.Net;
using CCW.Admin.Entities;
using CCW.Admin.Services;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;


namespace CCW.Admin.Tests;

internal class CosmosDbServiceTests
{
    protected string _databaseNameMock { get; }
    protected string _containerNameMock { get; }
    protected Mock<CosmosClient> _cosmosClientMock { get; }


    public CosmosDbServiceTests()
    {
        _databaseNameMock = "admin-database";
        _containerNameMock = "admin";
        _cosmosClientMock = new Mock<CosmosClient>();
    }

    [AutoMoqData]
    [Test]
    public async Task GetSettingsAsync_Should_Return_AgencyProfileSettings(
        AgencyProfileSettings agencyProfile
    )
    {
        // Arrange
        var agencies = new List<AgencyProfileSettings> { agencyProfile };

        var feedResponseMock = new Mock<FeedResponse<AgencyProfileSettings>>();
        feedResponseMock.SetupGet(p => p.Resource).Returns(agencies);
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(agencies.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<AgencyProfileSettings>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AgencyProfileSettings>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.GetSettingsAsync(default);

        // Assert
        result.Should().Be(agencyProfile);
        result.Should().BeOfType<AgencyProfileSettings>();
    }

    [AutoMoqData]
    [Test]
    public async Task GetSettingsAsync_Should_NotThrow_When_NotFound()
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AgencyProfileSettings>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.GetSettingsAsync(default);

        //  Act & Assert
        result.Should().Be(null);
    }

    [AutoMoqData]
    [Test]

    public async Task UpdateSettingsAsync_Should_Return_AgencyProfileSettings(
      AgencyProfileSettings agencyProfile,
      AgencyProfileSettings updatedAgencyProfile
    )
    {
        // Arrange
        var agencies = new List<AgencyProfileSettings> { agencyProfile };

        var feedResponseMock = new Mock<FeedResponse<AgencyProfileSettings>>();
        feedResponseMock.SetupGet(p => p.Resource).Returns(agencies);
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(agencies.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<AgencyProfileSettings>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AgencyProfileSettings>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        var responseMock = new Mock<ItemResponse<AgencyProfileSettings>>();
        responseMock.Setup(x => x.Resource).Returns(updatedAgencyProfile);

        container.Setup(x => x.UpsertItemAsync(
                It.IsAny<AgencyProfileSettings>(),
                It.IsAny<PartitionKey>(),
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.UpdateSettingsAsync(agencyProfile, default);

        // Assert
        result.Should().Be(updatedAgencyProfile);
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateSettingsAsync_Should_Create_AgencyProfileSettings_When_ResourceNotFound(
      AgencyProfileSettings agencyProfile
  )
    {
        // Arrange
        var agencies = new List<AgencyProfileSettings>();

        var feedResponseMock = new Mock<FeedResponse<AgencyProfileSettings>>();
        feedResponseMock.SetupGet(p => p.Resource).Returns(agencies);
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(agencies.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<AgencyProfileSettings>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<AgencyProfileSettings>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        var responseMock = new Mock<ItemResponse<AgencyProfileSettings>>();
        responseMock.Setup(x => x.Resource).Returns(agencyProfile);

        container.Setup(x => x.CreateItemAsync(
                It.IsAny<AgencyProfileSettings>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.UpdateSettingsAsync(agencyProfile, default);

        // Assert
        result.Should().Be(agencyProfile);
    }
}
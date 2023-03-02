using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;
using CCW.UserProfile.Services;
using System.Net;
using FluentAssertions;
using User = CCW.UserProfile.Entities.User;
using CCW.UserProfile.Entities;


namespace CCW.UserProfile.Tests;

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
    public async Task AddAsync_Should_Return_User_WhenCreate_NewUser(
        User user,
        User newUser,
        string email
    )
    {
        // Arrange
        user.UserEmail = email;
        var users = new List<User>( );

        var feedResponseMock = new Mock<FeedResponse<User>>();
        feedResponseMock.SetupGet(p => p.Resource).Returns(users);
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(users.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<User>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(false);

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<User>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        var responseMock = new Mock<ItemResponse<User>>();
        responseMock.Setup(x => x.Resource).Returns(newUser);

        container.Setup(x => x.CreateItemAsync(
                newUser,
                new PartitionKey(newUser.Id),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.AddAsync(newUser, default);

        // Assert
        result.Should().Be(newUser);
    }

    [AutoMoqData]
    [Test]
    public async Task AddAsync_Should_Return_SameUser_When_EmailUsedInThePast(
        User user,
        User newUser,
        string userId,
        string email
    )
    {
        // Arrange
        user.UserEmail = email;
        var users = new List<User> { user };

        var feedResponseMock = new Mock<FeedResponse<User>>();
        feedResponseMock.SetupGet(p => p.Resource).Returns(users);
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(users.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<User>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<User>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        var responseMock = new Mock<ItemResponse<User>>();
        responseMock.Setup(x => x.Resource).Returns(user);

        container.Setup(x => x.CreateItemAsync(
                user,
                new PartitionKey(user.Id),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.AddAsync(newUser, default);

        // Assert
        result.Should().Be(newUser);
    }

    [AutoMoqData]
    [Test]
    public async Task AddAsync_Should_Throw_When_UserExists(
        User user,
        string userId
    )
    {
        // Arrange
        var users = new List<User> { user };

        var feedResponseMock = new Mock<FeedResponse<User>>();
        feedResponseMock.SetupGet(p => p.Resource).Returns(users);
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(users.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<User>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<User>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        var responseMock = new Mock<ItemResponse<User>>();
        responseMock.Setup(x => x.Resource).Returns(user);

        container.Setup(x => x.CreateItemAsync(
                user,
                new PartitionKey(user.Id),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act & Assert
        await sut.Invoking(async x => await x.AddAsync(user, default)).Should()
            .ThrowAsync<Exception>().WithMessage("Email address already exists.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetAsync_Should_Return_User(
    User user,
    string email
    )
    {
        // Arrange
        var users = new List<User> { user };

        var feedResponseMock = new Mock<FeedResponse<User>>();
        feedResponseMock.SetupGet(p => p.Resource).Returns(users);
        feedResponseMock.Setup(x => x.GetEnumerator()).Returns(users.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<User>>();
        feedIteratorMock.Setup(f => f.HasMoreResults).Returns(true);
        feedIteratorMock
            .Setup(f => f.ReadNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedResponseMock.Object)
            .Callback(() => feedIteratorMock
                .Setup(f => f.HasMoreResults)
                .Returns(false));

        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<User>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(feedIteratorMock.Object);

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.GetAsync(email, default);

        // Assert
        result.Should().Be(user);
        result.Should().BeOfType<User>();
    }

    [AutoMoqData]
    [Test]
    public async Task GetAsync_Should_Throw_Null_When_NotFound(
        string email
    )
    {
        // Arrange
        var container = new Mock<Container>();
        container.Setup(x => x.GetItemQueryIterator<User>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        _cosmosClientMock.Setup(_ => _.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(container.Object);

        var sut = new CosmosDbService(_cosmosClientMock.Object, _databaseNameMock, _containerNameMock);

        // Act
        var result = await sut.GetAsync(email, default);

        // Assert
        result.Should().BeNull();
    }
}
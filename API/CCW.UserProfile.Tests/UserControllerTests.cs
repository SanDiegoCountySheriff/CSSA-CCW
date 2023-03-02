using CCW.UserProfile.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCW.UserProfile.Entities;
using CCW.UserProfile.Mappers;
using CCW.UserProfile.Models;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Net;
using FluentAssertions;
using User = CCW.UserProfile.Entities.User;
using System.Security.Claims;

namespace CCW.UserProfile.Tests;

internal class UserControllerTests
{
    protected Mock<ICosmosDbService> _cosmosDbService { get; }
    protected Mock<IMapper<string, UserProfileRequestModel, User>> _requestMapper { get; }
    protected Mock<IMapper<User, UserProfileResponseModel>> _responseMapper { get; }
    protected Mock<ILogger<UserController>> _logger { get; }

    public UserControllerTests()
    {
        _cosmosDbService = new Mock<ICosmosDbService>();
        _requestMapper = new Mock<IMapper<string, UserProfileRequestModel, User>>();
        _responseMapper = new Mock<IMapper<User, UserProfileResponseModel>>();
        _logger = new Mock<ILogger<UserController>>();
    }

    [AutoMoqData]
    [Test]
    public async Task Post_ShouldReturn_HttpResponseMessage_Ok_WhenFound(
     string userEmail,
     User dbResponse
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        _cosmosDbService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbResponse);

        var sut = new UserController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = sut.Post(userEmail);

        // Assert
        result.Should().BeOfType<HttpResponseMessage>();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [AutoMoqData]
    [Test]
    public async Task Post_ShouldReturn_HttpResponseMessage_NotFound_When_UserNotInTheDb(
        string userEmail,
        User dbResponse
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        _cosmosDbService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value:null!);

        var sut = new UserController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = sut.Post(userEmail);

        // Assert
        result.Should().BeOfType<HttpResponseMessage>();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [AutoMoqData]
    [Test]
    public async Task Post_ShouldReturn_HttpResponseMessage_ExpectationFail_When_Error(
        string userEmail,
        User dbResponse
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        _cosmosDbService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception message"));

        var sut = new UserController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = sut.Post(userEmail);

        // Assert
        result.Should().BeOfType<HttpResponseMessage>();
        result.StatusCode.Should().Be(HttpStatusCode.ExpectationFailed);
    }

    [AutoMoqData]
    [Test]
    public async Task Create_Should_Throw_When_InvalidToken(
        UserProfileRequestModel requestModel,
        UserProfileResponseModel responseModel,
        User dbResponse
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { }, "TestAuthentication"));

        dbResponse.UserEmail = requestModel.EmailAddress;
        _cosmosDbService.Setup(x => x.GetAsync(requestModel.EmailAddress, It.IsAny<CancellationToken>()))
            .Returns(value: null!);

        var sut = new UserController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        //  Act & Assert
        await sut.Invoking(async x => await x.Create(requestModel)).Should()
            .ThrowAsync<ArgumentNullException>().WithMessage("Invalid token. (Parameter 'userId')");
    }

    [AutoMoqData]
    [Test]
    public async Task Create_ShouldReturn_UserProfileResponseModel(
        UserProfileRequestModel requestModel,
        UserProfileResponseModel responseModel,
        User userData,
        User dbResponse
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        dbResponse.UserEmail = requestModel.EmailAddress;

        _requestMapper.Setup(x => x.Map(userData.Id, requestModel))
            .Returns(userData);

        _cosmosDbService.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userData);

        _responseMapper.Setup(x=>x.Map(userData)).Returns(responseModel);

        var sut = new UserController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = sut.Create(requestModel);

        // Assert
        result.Result.Should().BeEquivalentTo(responseModel);
    }

    [AutoMoqData]
    [Test]
    public async Task Create_Should_Throw_When_Error(
        UserProfileRequestModel requestModel
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        _cosmosDbService.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Exception"));

        var sut = new UserController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        //  Act & Assert
        await sut.Invoking(async x => await x.Create(requestModel)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to create new user.");
    }

}
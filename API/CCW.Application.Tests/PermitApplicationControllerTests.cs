using AutoFixture;
using AutoFixture.NUnit3;
using CCW.Application.Controllers;
using CCW.Application.Entities;
using CCW.Application.Models;
using Microsoft.Extensions.Logging;
using Moq;
using CCW.Application.Mappers;
using CCW.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace CCW.Application.Tests;

internal class PermitApplicationControllerTests
{
    protected Mock<ICosmosDbService> _cosmosDbService { get; }
    protected Mock<IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel>> _summaryPermitApplicationResponseMapper { get; }
    protected Mock<IMapper<PermitApplication, PermitApplicationResponseModel>> _permitApplicationResponseMapper { get; }
    protected Mock<IMapper<bool, PermitApplicationRequestModel, PermitApplication>> _permitApplicationMapper { get; }
    protected Mock<IMapper<PermitApplication, UserPermitApplicationResponseModel>> _userPermitApplicationResponseMapper
    {
        get;
    }
    protected Mock<IMapper<bool, string, UserPermitApplicationRequestModel, PermitApplication>> _userPermitApplicationMapper { get; }
    protected Mock<IMapper<History, HistoryResponseModel>> _historyMapper { get; }
    protected Mock<ILogger<PermitApplicationController>> _logger { get; }

    public PermitApplicationControllerTests()
    {
        _cosmosDbService = new Mock<ICosmosDbService>();
        _summaryPermitApplicationResponseMapper = new Mock<IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel>>();
        _permitApplicationResponseMapper = new Mock<IMapper<PermitApplication, PermitApplicationResponseModel>>();
        _permitApplicationMapper = new Mock<IMapper<bool, PermitApplicationRequestModel, PermitApplication>>();
        _userPermitApplicationResponseMapper =
            new Mock<IMapper<PermitApplication, UserPermitApplicationResponseModel>>();
        _userPermitApplicationMapper =
            new Mock<IMapper<bool, string, UserPermitApplicationRequestModel, PermitApplication>>();
        _historyMapper = new Mock<IMapper<History, HistoryResponseModel>>();
        _logger = new Mock<ILogger<PermitApplicationController>>();
    }

    [AutoMoqData]
    [Test]
    public async Task Create_ShouldReturn_AgencyProfileSettingsResponseModel(
    UserPermitApplicationRequestModel permitApplicationRequest,
    PermitApplication application,
    PermitApplicationResponseModel responseModel
)
    {
        // Arrange
        _userPermitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(), It.IsAny<string>(), permitApplicationRequest))
            .Returns(application);

        _cosmosDbService.Setup(x => x.AddAsync(application, It.IsAny<CancellationToken>()))
            .ReturnsAsync(application);

        _permitApplicationResponseMapper.Setup(x => x.Map(It.IsAny<PermitApplication>()))
            .Returns(responseModel);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Create(permitApplicationRequest);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        okResult?.Value.Should().BeOfType<PermitApplicationResponseModel>();
        okResult?.Value.Should().Be(responseModel);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task Create_Should_Throw_When_Error(
        UserPermitApplicationRequestModel permitApplicationRequest,
        PermitApplication application
    )
    {
        // Arrange
        _userPermitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(),It.IsAny<string>(), permitApplicationRequest))
            .Returns(application);

        _cosmosDbService.Setup(x => x.AddAsync(application, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Create(permitApplicationRequest)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to create permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetUserApplication_ShouldReturn_PermitApplicationResponseModel(
        PermitApplicationResponseModel response,
        PermitApplication permitApplication,
        string orderId,
        string userId,
        bool isOrderId = false,
        bool isComplete = false
            )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(userId, orderId, isComplete, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        _permitApplicationResponseMapper.Setup(x => x.Map(It.IsAny<PermitApplication>()))
            .Returns(response);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.GetUserApplication(orderId, isOrderId, isComplete);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        okResult?.Value.Should().BeOfType<PermitApplicationResponseModel>();
        okResult?.Value.Should().Be(response);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task GetUserApplication_Should_Throw_When_Error(
        string orderId,
        string userId,
        bool isOrderId = false,
        bool isComplete = false
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(userId, orderId, isComplete, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetUserApplication(orderId, isComplete)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve specific user permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetApplication_ShouldReturn_UserPermitApplicationResponseModel(
      UserPermitApplicationResponseModel response,
      PermitApplication permitApplication,
      string orderId,
      string userId,
      bool isOrderId = false,
      bool isComplete = false
  )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(userId, orderId, isComplete, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        _userPermitApplicationResponseMapper.Setup(x => x.Map(It.IsAny<PermitApplication>()))
            .Returns(response);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.GetApplication(orderId, isComplete);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        okResult?.Value.Should().BeOfType<UserPermitApplicationResponseModel>();
        okResult?.Value.Should().Be(response);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task GetApplication_Should_Throw_When_Error(
        string orderId,
        string userId,
        bool isOrderId = false,
        bool isComplete = false
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(userId, orderId,
               isComplete, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetApplication(orderId, isComplete)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve user permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetUserApplications_ShouldReturn_PermitApplicationResponseModel(
       PermitApplicationResponseModel response,
       IEnumerable<PermitApplication> permitApplication,
       string userEmailOrOrderId
    )
    {
        // Arrange
        Guid id = new Guid();
        _cosmosDbService.Setup(x => x.GetAllUserApplicationsAsync(userEmailOrOrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        _permitApplicationResponseMapper.Setup(x => x.Map(It.IsAny<PermitApplication>()))
            .Returns((PermitApplication a) => new PermitApplicationResponseModel { Id = id });

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.GetUserApplications(userEmailOrOrderId);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
    }

    [AutoMoqData]
    [Test]
    public async Task GetUserApplications_Should_Throw_When_Error(
        string userEmailOrOrderId
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAllUserApplicationsAsync(userEmailOrOrderId, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetUserApplications(userEmailOrOrderId)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve all user permit applications.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetApplications_ShouldReturn_PermitApplicationResponseModel(
   UserPermitApplicationResponseModel response,
   IEnumerable<PermitApplication> permitApplication,
   string userEmailOrOrderId
)
    {
        // Arrange
        Guid id = new Guid();
        _cosmosDbService.Setup(x => x.GetAllUserApplicationsAsync(userEmailOrOrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        _permitApplicationResponseMapper.Setup(x => x.Map(It.IsAny<PermitApplication>()))
            .Returns((PermitApplication a) => new PermitApplicationResponseModel { Id = id });

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.GetApplications(userEmailOrOrderId);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
    }

    [AutoMoqData]
    [Test]
    public async Task GetApplications_Should_Throw_When_Error(
        string userEmailOrOrderId
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAllUserApplicationsAsync(userEmailOrOrderId, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetApplications(userEmailOrOrderId)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve user permit applications.");
    }


    [AutoMoqData]
    [Test]
    public async Task GetHistory_ShouldReturn_IEnumerable_HistoryResponseModel(
        IEnumerable<History> historyList,
        string applicationIdOrOrderId,
        bool isOrderId = false
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetApplicationHistoryAsync(applicationIdOrOrderId, It.IsAny<CancellationToken>(), isOrderId))
           .ReturnsAsync(historyList);

        _historyMapper.Setup(m => m.Map(It.IsAny<History>()))
            .Returns((History h) => new HistoryResponseModel() { ChangeMadeBy = h.ChangeMadeBy + "blue" });

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.GetHistory(applicationIdOrOrderId, isOrderId);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        var expected = historyList.Select(x => x.ChangeMadeBy + "blue");
        var returned = (IEnumerable<HistoryResponseModel>)okResult.Value;
        returned?.Select(y => y.ChangeMadeBy).Should().BeEquivalentTo(expected);
    }

    [AutoMoqData]
    [Test]
    public async Task GetHistory_Should_Throw_When_Error(
        string applicationIdOrOrderId,
        bool isOrderId = false
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetApplicationHistoryAsync(applicationIdOrOrderId, It.IsAny<CancellationToken>(), isOrderId))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetHistory(applicationIdOrOrderId, isOrderId)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve permit application history.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetHistory_ShouldReturn_IEnumerable_HistoryResponseModel(
        IEnumerable<SummarizedPermitApplicationResponseModel> response,
        IEnumerable<SummarizedPermitApplication> sumarizedApplications,
        IEnumerable<PermitApplication> applications
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAllApplicationsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(applications);

        _summaryPermitApplicationResponseMapper.Setup(m => m.Map(It.IsAny<SummarizedPermitApplication>()))
            .Returns((SummarizedPermitApplication h) => new SummarizedPermitApplicationResponseModel() { FirstName = h.FirstName + "green" });

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.GetAll();
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        var expected = sumarizedApplications.Select(x => x.FirstName + "green");
        var returned = (IEnumerable<SummarizedPermitApplicationResponseModel>)okResult.Value;
        returned?.Select(y => y.FirstName).Should().BeEquivalentTo(expected);
    }

    [AutoMoqData]
    [Test]
    public async Task GetAll_Should_Return_SummarizedPermitApplicationResponseModel()
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAllApplicationsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAll()).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve all permit applications.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetAll_Should_Throw_When_Error()
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAllApplicationsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAll()).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve all permit applications.");
    }

    [AutoMoqData]
    [Test]
    public async Task Search_ShouldReturn_IEnumerable_HistoryResponseModel(
       IEnumerable<SummarizedPermitApplication> applicationList,
       string searchValue
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.SearchApplicationsAsync(searchValue, It.IsAny<CancellationToken>()))
           .ReturnsAsync(applicationList);

        _summaryPermitApplicationResponseMapper.Setup(m => m.Map(It.IsAny<SummarizedPermitApplication>()))
            .Returns((SummarizedPermitApplication h) => new SummarizedPermitApplicationResponseModel() { FirstName = h.FirstName + "blue" });

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Search(searchValue);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        var expected = applicationList.Select(x => x.FirstName + "blue");
        var returned = (IEnumerable<SummarizedPermitApplicationResponseModel>)okResult.Value;
        returned?.Select(y => y.FirstName).Should().BeEquivalentTo(expected);
    }

    [AutoMoqData]
    [Test]
    public async Task Search_Should_Throw_When_Error(
        string searchValue
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.SearchApplicationsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Search(searchValue)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to search all permit applications.");
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateUserApplication_ShouldReturn_NoContent(
      PermitApplicationRequestModel permitApplicationRequest,
      PermitApplication application
    )
    {
        // Arrange
        _permitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(), permitApplicationRequest))
            .Returns(application);

        _cosmosDbService.Setup(x => x.UpdateUserApplicationAsync(application, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.UpdateUserApplication(permitApplicationRequest);

        // Assert
        var okResult = result.Should().BeOfType<NoContentResult>().Subject;
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateUserApplication_Should_Throw_When_Error(
        PermitApplicationRequestModel permitApplicationRequest,
        PermitApplication application
    )
    {
        // Arrange
        _permitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(), permitApplicationRequest))
            .Returns(application);

        _cosmosDbService.Setup(x => x.UpdateUserApplicationAsync(application, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UpdateUserApplication(permitApplicationRequest)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to update permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateApplication_ShouldReturn_OK(
     UserPermitApplicationRequestModel permitApplicationRequest,
     PermitApplication permitApplication
    )
    {
        // Arrange
        permitApplicationRequest.Application.IsComplete = false;
        permitApplicationRequest.Application.OrderId = "ABC12345";
        permitApplication.Application.IsComplete = false;
        permitApplication.Application.OrderId = "ABC12345";

        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<bool>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        _cosmosDbService.Setup(x => x.UpdateApplicationAsync(permitApplication, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _userPermitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(), It.IsAny<string>(), permitApplicationRequest))
            .Returns(permitApplication);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.UpdateApplication(permitApplicationRequest);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateApplication_Should_Throw_When_ApplicationIsComplete(
        UserPermitApplicationRequestModel permitApplicationRequest,
        PermitApplication permitApplication)
    {
        // Arrange
        permitApplicationRequest.Application.IsComplete = true;
        permitApplicationRequest.Application.OrderId = "ABC12345";
        permitApplication.Application.IsComplete = true;
        permitApplication.Application.OrderId = "ABC12345";

        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(It.IsAny<string>(), It.IsAny<string>(),
                 It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        _cosmosDbService.Setup(x => x.UpdateApplicationAsync(permitApplication, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _userPermitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(), It.IsAny<string>(), permitApplicationRequest))
            .Returns(permitApplication);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UpdateApplication(permitApplicationRequest)).Should()
            .ThrowAsync<Exception>().WithMessage("Permit application submitted changes cannot be made.");
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateApplication_Should_ReturnNotFound_When_ApplicationNotFound(
        [Frozen] UserPermitApplicationRequestModel permitApplicationRequest,
        PermitApplication application
    )
    {
        // Arrange
        permitApplicationRequest.Application.IsComplete = false;

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.UpdateApplication(permitApplicationRequest);

        // Assert
        var okResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        okResult.Value.Should().Be("Permit application cannot be found.");
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateApplication_Should_Throw_When_Error(
        UserPermitApplicationRequestModel permitApplicationRequest)
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(It.IsAny<string>(),
            It.IsAny<string>(),
                It.IsAny<bool>(),It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UpdateApplication(permitApplicationRequest)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to update permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteApplication_ShouldReturn_NoContent(
       string orderId,
       string userId,
        PermitApplication permitApplication
    )
    {
        // Arrange
        permitApplication.Application.IsComplete = false;
        permitApplication.Application.OrderId = "ABC12345";

        _cosmosDbService.Setup(x => x.GetLastApplicationAsync( It.IsAny<string>(),
        It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        _cosmosDbService.Setup(x => x.DeleteApplicationAsync(userId, orderId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.DeleteApplication(orderId);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteApplication_ShouldReturn_NotFound_WhenAppNotExists(
        string orderId
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null!);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.DeleteApplication(orderId);

        // Assert
        var okResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        okResult.Value.Should().Be("Permit application cannot be found or has been completed and no longer can be deleted.");
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteApplication_ShouldReturn_NotFound_WhenAppCompleted(
        PermitApplication permitApplication,
        string orderId
    )
    {
        // Arrange
        permitApplication.Application.IsComplete = true;
        permitApplication.Application.OrderId = "ABC12345";

        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(It.IsAny<string>(),
                It.IsAny<string>(),It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.DeleteApplication(orderId);

        // Assert
        var okResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        okResult.Value.Should().Be("Permit application submitted changes cannot be deleted.");
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteApplication_Should_Throw_When_Error(
        string orderId
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(It.IsAny<string>(),
                It.IsAny<string>(),It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _userPermitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _userPermitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DeleteApplication(orderId)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to delete permit application.");
    }
}
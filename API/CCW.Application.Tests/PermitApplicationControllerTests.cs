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
    protected Mock<IMapper<History, HistoryResponseModel>> _historyMapper { get; }
    protected Mock<ILogger<PermitApplicationController>> _logger { get; }

    public PermitApplicationControllerTests()
    {
        _cosmosDbService = new Mock<ICosmosDbService>();
        _summaryPermitApplicationResponseMapper = new Mock<IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel>>();
        _permitApplicationResponseMapper = new Mock<IMapper<PermitApplication, PermitApplicationResponseModel>>();
        _permitApplicationMapper = new Mock<IMapper<bool, PermitApplicationRequestModel, PermitApplication>>();
        _historyMapper = new Mock<IMapper<History, HistoryResponseModel>>();
        _logger = new Mock<ILogger<PermitApplicationController>>();
    }

    [AutoMoqData]
    [Test]
    public async Task Create_ShouldReturn_AgencyProfileSettingsResponseModel(
    PermitApplicationRequestModel permitApplicationRequest,
    PermitApplication application,
    PermitApplicationResponseModel responseModel
)
    {
        // Arrange
        _permitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(), permitApplicationRequest))
            .Returns(application);

        _cosmosDbService.Setup(x => x.AddAsync(application, It.IsAny<CancellationToken>()))
            .ReturnsAsync(application);

        _permitApplicationResponseMapper.Setup(x => x.Map(It.IsAny<PermitApplication>()))
            .Returns(responseModel);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
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
        PermitApplicationRequestModel permitApplicationRequest,
        PermitApplication application
    )
    {
        // Arrange
        _permitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(), permitApplicationRequest))
            .Returns(application);

        _cosmosDbService.Setup(x => x.AddAsync(application, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Create(permitApplicationRequest)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to create permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task Get_ShouldReturn_PermitApplicationResponseModel(
        PermitApplicationResponseModel response,
        PermitApplication permitApplication,
        string userEmailOrOrderId,
        bool isOrderId = false,
        bool isComplete = false
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(userEmailOrOrderId, isOrderId, isComplete, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        _permitApplicationResponseMapper.Setup(x => x.Map(It.IsAny<PermitApplication>()))
            .Returns(response);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Get(userEmailOrOrderId, isOrderId, isComplete);
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
    public async Task Get_Should_Throw_When_Error(
        string userEmailOrOrderId,
        bool isOrderId = false,
        bool isComplete = false
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetLastApplicationAsync(userEmailOrOrderId, isOrderId, isComplete, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Get(userEmailOrOrderId, isOrderId, isComplete)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve permit application.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetUserEmail_ShouldReturn_PermitApplicationResponseModel(
       PermitApplicationResponseModel response,
       IEnumerable<PermitApplication> permitApplication,
       string userEmailOrOrderId,
       Guid expectedGuid
    )
    {
        // Arrange
        Guid id = new Guid();
        _cosmosDbService.Setup(x => x.GetAllUserApplicationsAsync(userEmailOrOrderId,It.IsAny<CancellationToken>()))
            .ReturnsAsync(permitApplication);

        _permitApplicationResponseMapper.Setup(x => x.Map(It.IsAny<PermitApplication>()))
            .Returns((PermitApplication a) => new PermitApplicationResponseModel(){ Id = id});

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Get(userEmailOrOrderId);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
    }

    [AutoMoqData]
    [Test]
    public async Task GetUserEmail_Should_Throw_When_Error(
        string userEmailOrOrderId
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAllUserApplicationsAsync(userEmailOrOrderId,It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Get(userEmailOrOrderId)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve all user permit applications.");
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
            _permitApplicationMapper.Object,
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
            _permitApplicationMapper.Object,
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
        IEnumerable<SummarizedPermitApplication> sumarizedApplications
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAllApplicationsAsync(It.IsAny<CancellationToken>()))
           .ReturnsAsync(sumarizedApplications);

        _summaryPermitApplicationResponseMapper.Setup(m => m.Map(It.IsAny<SummarizedPermitApplication>()))
            .Returns((SummarizedPermitApplication h) => new SummarizedPermitApplicationResponseModel() { FirstName = h.FirstName + "green" });

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
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
        _cosmosDbService.Setup(x => x.GetAllApplicationsAsync(It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
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
        _cosmosDbService.Setup(x => x.GetAllApplicationsAsync(It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAll()).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve all permit applications.");
    }

    [AutoMoqData]
    [Test]
    public async Task Update_ShouldReturn_NoContent(
      PermitApplicationRequestModel permitApplicationRequest,
      PermitApplication application
    )
    {
        // Arrange
        _permitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(), permitApplicationRequest))
            .Returns(application);

        _cosmosDbService.Setup(x => x.UpdateAsync(application, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Update(permitApplicationRequest);

        // Assert
        var okResult = result.Should().BeOfType<NoContentResult>().Subject;
    }

    [AutoMoqData]
    [Test]
    public async Task Update_Should_Throw_When_Error(
        PermitApplicationRequestModel permitApplicationRequest,
        PermitApplication application
    )
    {
        // Arrange
        _permitApplicationMapper.Setup(x => x.Map(It.IsAny<bool>(), permitApplicationRequest))
            .Returns(application);

        _cosmosDbService.Setup(x => x.UpdateAsync(application, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new PermitApplicationController(
            _cosmosDbService.Object,
            _summaryPermitApplicationResponseMapper.Object,
            _permitApplicationResponseMapper.Object,
            _permitApplicationMapper.Object,
            _historyMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Update(permitApplicationRequest)).Should()
                .ThrowAsync<Exception>().WithMessage("An error occur while trying to update permit application.");
    }
}
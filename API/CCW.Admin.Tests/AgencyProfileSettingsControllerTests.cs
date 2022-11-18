using Microsoft.Extensions.Logging;
using Moq;
using CCW.Admin.Mappers;
using CCW.Admin.Services;
using CCW.Admin.Entities;
using CCW.Admin.Models;
using CCW.Admin.Controllers;
using FluentAssertions;

namespace CCW.Admin.Tests;

internal class AgencyProfileSettingsControllerTests
{
    protected Mock<ICosmosDbService> _cosmosDbService { get; }
    protected Mock<IMapper<AgencyProfileSettingsRequestModel, AgencyProfileSettings>> _requestMapper { get; }
    protected Mock<IMapper<AgencyProfileSettings, AgencyProfileSettingsResponseModel>> _responseMapper { get; }
    protected Mock<ILogger<SystemSettingsController>> _logger { get; }

    public AgencyProfileSettingsControllerTests()
    {
        _cosmosDbService = new Mock<ICosmosDbService>();
        _requestMapper = new Mock<IMapper<AgencyProfileSettingsRequestModel, AgencyProfileSettings>>();
        _responseMapper = new Mock<IMapper<AgencyProfileSettings, AgencyProfileSettingsResponseModel>>();
        _logger = new Mock<ILogger<SystemSettingsController>>();
    }

    [AutoMoqData]
    [Test]
    public async Task Get_ShouldReturn_AgencyProfileSettingsResponseModel(
        AgencyProfileSettings dbResponse,
        AgencyProfileSettingsResponseModel response
        )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetSettingsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbResponse);

        _responseMapper.Setup(x=>x.Map(It.IsAny<AgencyProfileSettings>()))
            .Returns(response);

        var sut = new SystemSettingsController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Get();

        // Assert
        result.Should().Be(response);
    }

    [AutoMoqData]
    [Test]
    public async Task Get_Should_Throw_When_Error(
        AgencyProfileSettings dbResponse
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetSettingsAsync(It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new SystemSettingsController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Get()).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve agency settings.");
    }

    [AutoMoqData]
    [Test]
    public async Task Update_ShouldReturn_AgencyProfileSettingsResponseModel(
        AgencyProfileSettingsRequestModel agencyProfileRequest,
        AgencyProfileSettings agencyProfileSettings,
        AgencyProfileSettingsResponseModel responseModel
    )
    {
        // Arrange
        _requestMapper.Setup(x => x.Map(agencyProfileRequest))
            .Returns(agencyProfileSettings);

        _cosmosDbService.Setup(x => x.GetSettingsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(agencyProfileSettings);

        _responseMapper.Setup(x => x.Map(It.IsAny<AgencyProfileSettings>()))
            .Returns(responseModel);

        var sut = new SystemSettingsController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Update(agencyProfileRequest);

        // Assert
        result.Should().Be(responseModel);
    }

    [AutoMoqData]
    [Test]
    public async Task Update_Should_Throw_When_Error(
        AgencyProfileSettingsRequestModel agencyProfileRequest,
        AgencyProfileSettings agencyProfileSettings
    )
    {
        // Arrange
        _requestMapper.Setup(x => x.Map(It.IsAny<AgencyProfileSettingsRequestModel>()))
            .Returns(agencyProfileSettings);

        _cosmosDbService.Setup(x => x.UpdateSettingsAsync(agencyProfileSettings, It.IsAny<CancellationToken>()))
            .Throws(new Exception("Exception"));

        var sut = new SystemSettingsController(
            _cosmosDbService.Object,
            _requestMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Update(agencyProfileRequest)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve agency settings.");
    }
}
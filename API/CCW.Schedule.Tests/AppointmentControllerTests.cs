using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Azure;
using Azure.Core;
using CCW.Schedule.Controllers;
using CCW.Schedule.Entities;
using CCW.Schedule.Mappers;
using CCW.Schedule.Models;
using CCW.Schedule.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Extensions.Logging;
using Moq;

namespace CCW.Schedule.Tests;

internal class AppointmentControllerTests
{
    protected Mock<ICosmosDbService> _cosmosDbService { get; }
    protected Mock<IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow>> _requestCreateApptMapper { get; }
    protected Mock<IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow>> _requestUpdateApptMapper { get; }
    protected Mock<IMapper<AppointmentWindow, AppointmentWindowResponseModel>> _responseMapper { get; }
    protected Mock<ILogger<AppointmentController>> _logger { get; }

    public AppointmentControllerTests()
    {
        _cosmosDbService = new Mock<ICosmosDbService>();
        _requestCreateApptMapper = new Mock<IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow>>();
        _requestUpdateApptMapper = new Mock<IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow>>();
        _responseMapper = new Mock<IMapper<AppointmentWindow, AppointmentWindowResponseModel>>();
        _logger = new Mock<ILogger<AppointmentController>>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadFile_ShouldReturn_Success(IFormFile fileToUpload)
    {

    }

    [AutoMoqData]
    [Test]
    public async Task UploadFile_ShouldReturn_BadRequest_WhenError(IFormFile fileToUpload)
    {
     
    }

    [AutoMoqData]
    [Test]
    public async Task GetAppointmentTimes_ShouldReturn_IEnumerable_AppointmentWindowResponseModel()
    {
      
    }

    [AutoMoqData]
    [Test]
    public async Task GetAppointmentTimes_ShouldReturn_BadRequest_WhenError()
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAvailableTimesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.GetAppointmentTimes();
        var badResult = result as ObjectResult;

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        badResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badResult?.Value.Should().Be("An error occur. Please ty again.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetAll_ShouldReturn_IEnumerable_AppointmentWindowResponseModel(
        AppointmentWindow appointment,
        AppointmentWindowResponseModel responseAppt,
        IEnumerable<AppointmentWindowResponseModel> response)
    {
        // Arrange
        var dbResponse = new List<AppointmentWindow> { appointment };
    
        dbResponse.Select(x=> _responseMapper.Setup(y=>y.Map(x)));

        _cosmosDbService.Setup(x => x.GetAllBookedAppointmentsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbResponse);

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.GetAll();
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        okResult?.Value.Should().BeOfType<AppointmentWindowResponseModel>();
        okResult?.Value.Should().Be(response);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task GetAll_ShouldReturn_BadRequest_WhenError()
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAllBookedAppointmentsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.GetAll();
        var badResult = result as ObjectResult;

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        badResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badResult?.Value.Should().Be("An error occur. Please ty again.");
    }

    [AutoMoqData]
    [Test]
    public async Task Get_ShouldReturn_AppointmentWindowResponseModel(
        string applicationId,
        AppointmentWindow dbResponse,
        AppointmentWindowResponseModel response
        )
    {
        // Arrange
        _responseMapper.Setup(x => x.Map(dbResponse)).Returns(response);

        _cosmosDbService.Setup(x => x.GetAsync(applicationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbResponse);

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Get(applicationId);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        okResult?.Value.Should().BeOfType<AppointmentWindowResponseModel>();
        okResult?.Value.Should().Be(response);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task Get_ShouldReturn_BadRequest_WhenError(string applicationId)
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAsync(applicationId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Get(applicationId);
        var badResult = result as ObjectResult;

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        badResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badResult?.Value.Should().Be("An error occur. Please ty again.");
    }

    [AutoMoqData]
    [Test]
    public async Task Create_ShouldReturn_AppointmentWindowResponseModel(
        AppointmentWindow appointment,
        AppointmentWindowCreateRequestModel request
        )
    {
        // Arrange
        _requestCreateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.AddAsync(appointment, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Create(request);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        okResult?.Value.Should().BeOfType<AppointmentWindowResponseModel>();
        okResult?.Value.Should().Be(appointment);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task Create_ShouldReturn_BadRequest_WhenError(
        AppointmentWindow appointment,
        AppointmentWindowCreateRequestModel request
        )
    {
        // Arrange
        _requestCreateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.AddAsync(appointment, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Create(request);
        var badResult = result as ObjectResult;

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        badResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badResult?.Value.Should().Be("An error occur. Please ty again.");
    }

    [AutoMoqData]
    [Test]
    public async Task Update_ShouldReturn_AppointmentWindowResponseModel(
        AppointmentWindow appointment,
        AppointmentWindowUpdateRequestModel request
    )
    {
        // Arrange
        _requestUpdateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.UpdateAsync(appointment, It.IsAny<CancellationToken>()));

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Update(request);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task Update_ShouldReturn_BadRequest_WhenError(
        AppointmentWindow appointment,
        AppointmentWindowUpdateRequestModel request
        )
    {
        // Arrange
        _requestUpdateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.UpdateAsync(appointment, It.IsAny<CancellationToken>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Update(request);
        var badResult = result as ObjectResult;

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        badResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badResult?.Value.Should().Be("An error occur. Please ty again.");
    }

    [AutoMoqData]
    [Test]
    public async Task Delete_ShouldReturn_AppointmentWindowResponseModel(string appointmentId)
    {
        // Arrange
        _cosmosDbService.Setup(x => x.DeleteAsync(appointmentId, It.IsAny<CancellationToken>()));

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Delete(appointmentId);
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task Delete_ShouldReturn_BadRequest_WhenError(string appointmentId)
    {
        // Arrange
        _cosmosDbService.Setup(x => x.DeleteAsync(appointmentId, It.IsAny<CancellationToken>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Delete(appointmentId);
        var badResult = result as ObjectResult;

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        badResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badResult?.Value.Should().Be("An error occur. Please ty again.");
    }
}
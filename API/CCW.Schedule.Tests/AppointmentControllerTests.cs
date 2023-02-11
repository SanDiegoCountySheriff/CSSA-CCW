using System.IO;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using Azure;
using Azure.Core;
using CCW.Schedule.Clients;
using CCW.Schedule.Controllers;
using CCW.Schedule.Entities;
using CCW.Schedule.Mappers;
using CCW.Schedule.Models;
using CCW.Schedule.Services;
using CsvHelper;
using Dia2Lib;
using FastSerialization;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace CCW.Schedule.Tests;

internal class AppointmentControllerTests
{
    protected Mock<IApplicationServiceClient> _applicationHttpClient { get; }
    protected Mock<ICosmosDbService> _cosmosDbService { get; }
    protected Mock<IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow>> _requestCreateApptMapper { get; }
    protected Mock<IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow>> _requestUpdateApptMapper { get; }
    protected Mock<IMapper<AppointmentWindow, AppointmentWindowResponseModel>> _responseMapper { get; }
    protected Mock<ILogger<AppointmentController>> _logger { get; }

    public AppointmentControllerTests()
    {
        _applicationHttpClient = new Mock<IApplicationServiceClient>();
        _cosmosDbService = new Mock<ICosmosDbService>();
        _requestCreateApptMapper = new Mock<IMapper<AppointmentWindowCreateRequestModel, AppointmentWindow>>();
        _requestUpdateApptMapper = new Mock<IMapper<AppointmentWindowUpdateRequestModel, AppointmentWindow>>();
        _responseMapper = new Mock<IMapper<AppointmentWindow, AppointmentWindowResponseModel>>();
        _logger = new Mock<ILogger<AppointmentController>>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadFile_ShouldReturn_Success(
        IFormFile fileToUpload,
        IEnumerable<AppointmentUploadModel> appointmentUploadModels)
    {
        // Arrange
        List<AppointmentWindow> appointments = new List<AppointmentWindow>();
        _cosmosDbService.Setup(x => x.AddAvailableTimesAsync(appointments, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var content = "Hello World from a Fake File";
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.UploadFile(file);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadFile_Should_Throw_Error(IFormFile fileToUpload)
    {
        // Arrange
        var stream = new Mock<StreamReader>();
        stream.Setup(x => x.BaseStream)
            .Throws(new Exception("Exception"));

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UploadFile(fileToUpload)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to upload file.");
    }

    [AutoMoqData]
    [Test]
    public async Task GetAppointmentTimes_ShouldReturn_IEnumerable_AppointmentWindowResponseModel(
        AppointmentWindow appointment,
        IEnumerable<AppointmentWindowResponseModel> response
        )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        var dbResponse = new List<AppointmentWindow> { appointment };

        _cosmosDbService.Setup(x => x.GetAvailableTimesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbResponse);

        _responseMapper.Setup(m => m.Map(It.IsAny<AppointmentWindow>()))
            .Returns((AppointmentWindow h) => new AppointmentWindowResponseModel() { ApplicationId = h.ApplicationId + "blue" });

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.GetAppointmentTimes();
        var okResult = result as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkObjectResult);
        var expected = dbResponse.Select(x => x.ApplicationId + "blue");
        var returned = (IEnumerable<AppointmentWindowResponseModel>)okResult.Value;
        returned?.Select(y => y.ApplicationId).Should().BeEquivalentTo(expected);
    }

    [AutoMoqData]
    [Test]
    public async Task GetAppointmentTimes_Should_Throw_WhenError()
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAvailableTimesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAppointmentTimes()).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve available appointments.");
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

        _cosmosDbService.Setup(x => x.GetAllBookedAppointmentsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbResponse);

        _responseMapper.Setup(m => m.Map(It.IsAny<AppointmentWindow>()))
            .Returns((AppointmentWindow h) => new AppointmentWindowResponseModel() { ApplicationId = h.ApplicationId + "blue" });

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
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
        var expected = dbResponse.Select(x => x.ApplicationId + "blue");
        var returned = (IEnumerable<AppointmentWindowResponseModel>)okResult.Value;
        returned?.Select(y => y.ApplicationId).Should().BeEquivalentTo(expected);
    }

    [AutoMoqData]
    [Test]
    public async Task GetAll_Should_Throw_WhenError()
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAllBookedAppointmentsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAll()).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve all booked appointments.");
    }

    [AutoMoqData]
    [Test]
    public async Task Get_ShouldReturn_AppointmentWindowResponseModel(
        string applicationId,
        Guid userId,
        AppointmentWindow dbResponse,
        AppointmentWindowResponseModel response
        )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        _responseMapper.Setup(x => x.Map(dbResponse)).Returns(response);

        _cosmosDbService.Setup(x => x.GetAsync(applicationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbResponse);

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

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
    public async Task Get_Should_Throw_WhenError(string applicationId)
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAsync(applicationId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Get(applicationId)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to retrieve appointment.");
    }

    [AutoMoqData]
    [Test]
    public async Task Create_ShouldReturn_AppointmentWindowResponseModel(
        AppointmentWindow appointment,
        AppointmentWindowCreateRequestModel request,
        AppointmentWindowResponseModel response
        )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        _requestCreateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.AddAsync(appointment, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        _responseMapper.Setup(x => x.Map(It.IsAny<AppointmentWindow>()))
            .Returns(response);

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.Create(request);
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
    public async Task Create_Should_Throw_WhenError(
        AppointmentWindow appointment,
        AppointmentWindowCreateRequestModel request
        )
    {
        // Arrange
        _requestCreateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.AddAsync(appointment, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Create(request)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to create appointment.");
    }

    [AutoMoqData]
    [Test]
    public async Task Update_ShouldReturn_Ok(
        AppointmentWindow appointment,
        AppointmentWindowUpdateRequestModel request
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        _requestUpdateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.UpdateAsync(appointment, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.Update(request);
        var okResult = result as OkResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkResult);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task Update_Should_Throw_WhenError(
        AppointmentWindow appointment,
        AppointmentWindowUpdateRequestModel request
        )
    {
        // Arrange
        _requestUpdateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.UpdateAsync(appointment, It.IsAny<CancellationToken>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.Update(request)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to update appointment.");
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateUserAppointment_ShouldReturn_Ok(
     AppointmentWindow appointment,
     AppointmentWindowUpdateRequestModel request
 )
    {
        // Arrange
        _requestUpdateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.UpdateAsync(appointment, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };

        _applicationHttpClient.Setup(x => x.UpdateApplicationAppointmentAsync(
                It.IsAny<string>(),
                It.IsAny<DateTime>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponseMessage);

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.UpdateUserAppointment(request);
        var okResult = result as OkResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkResult);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateUserAppointment_ShouldReturn_BadRequest_When_ApplicationClientFails(
        AppointmentWindow appointment,
        AppointmentWindowUpdateRequestModel request
    )
    {
        // Arrange
        _requestUpdateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.UpdateAsync(appointment, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadGateway
        };

        _applicationHttpClient.Setup(x => x.UpdateApplicationAppointmentAsync(
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponseMessage);

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.UpdateUserAppointment(request);
        var responseResult = result as BadRequestResult;

        // Assert
        Assert.NotNull(responseResult);
        Assert.True(responseResult is BadRequestResult);
        responseResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateUserAppointment_Should_Throw_WhenError(
        AppointmentWindow appointment,
        AppointmentWindowUpdateRequestModel request
        )
    {
        // Arrange
        _requestUpdateApptMapper.Setup(x => x.Map(request)).Returns(appointment);

        _cosmosDbService.Setup(x => x.UpdateAsync(appointment, It.IsAny<CancellationToken>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UpdateUserAppointment(request)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to update appointment.");
    }

    [AutoMoqData]
    [Test]
    public async Task ReopenSlot_ShouldReturn_Ok(
     AppointmentWindow appointment
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAppointmentByIdAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        _cosmosDbService.Setup(x => x.UpdateAsync(appointment,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };

        _applicationHttpClient.Setup(x => x.RemoveApplicationAppointmentAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponseMessage);

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.ReopenSlot(appointment.Id.ToString());
        var okResult = result as OkResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkResult);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task ReopenSlot_ShouldReturn_BadRequest_When_ApplicationClientFails(
        AppointmentWindow appointment
    )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAppointmentByIdAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        _cosmosDbService.Setup(x => x.UpdateAsync(appointment,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadGateway
        };

        _applicationHttpClient.Setup(x => x.RemoveApplicationAppointmentAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponseMessage);

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.ReopenSlot(appointment.Id.ToString());
        var responseResult = result as BadRequestResult;

        // Assert
        Assert.NotNull(responseResult);
        Assert.True(responseResult is BadRequestResult);
        responseResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }


    [AutoMoqData]
    [Test]
    public async Task ReopenSlot_Should_Throw_WhenError(
        string appointmentId
        )
    {
        // Arrange
        _cosmosDbService.Setup(x => x.GetAppointmentByIdAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.ReopenSlot(appointmentId)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to delete appointment.");
    }

    [AutoMoqData]
    [Test]
    public async Task Delete_ShouldReturn_Ok(string appointmentId)
    {
        // Arrange
        _cosmosDbService.Setup(x => x.DeleteAsync(appointmentId, default))
            .Returns(Task.CompletedTask);

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        var result = await sut.Delete(appointmentId);
        var okResult = result as OkResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.True(okResult is OkResult);
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [AutoMoqData]
    [Test]
    public async Task Delete_Should_Throw_WhenError(string appointmentId)
    {
        // Arrange
        _cosmosDbService.Setup(x => x.DeleteAsync(appointmentId, It.IsAny<CancellationToken>()))
            .Throws(new CosmosException("Exception", HttpStatusCode.NotFound, 404, null, Double.MinValue));

        var sut = new AppointmentController(
            _applicationHttpClient.Object,
            _cosmosDbService.Object,
            _requestCreateApptMapper.Object,
            _requestUpdateApptMapper.Object,
            _responseMapper.Object,
            _logger.Object);

        // Act
        //  Act & Assert
        await sut.Invoking(async x => await x.Delete(appointmentId)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to delete appointment.");
    }
}
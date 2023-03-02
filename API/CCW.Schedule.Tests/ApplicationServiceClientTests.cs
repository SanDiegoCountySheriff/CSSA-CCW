using CCW.Schedule.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCW.Schedule.Entities;
using CCW.Schedule.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Text.Json;
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;
using Microsoft.Extensions.Configuration;
using Moq.Protected;

namespace CCW.Schedule.Tests;

public class ApplicationServiceClientTests
{

    [AutoMoqData]
    [Test]
    public async Task RemoveApplicationAppointmentAsync_ShouldReturn_Ok(
        string applicationId
    )
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            }).Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://google.com/")
        };

        var iConfigurationMock = new Mock<IConfiguration>();
        iConfigurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns("test");

            var sut = new ApplicationServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.RemoveApplicationAppointmentAsync(applicationId, default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [AutoMoqData]
    [Test]
    public async Task UpdateApplicationAppointmentAsync_ShouldReturn_Ok(
        string applicationId,
        DateTime appointmentDate
    )
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            }).Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://google.com/"),
        };

        var iConfigurationMock = new Mock<IConfiguration>();
        iConfigurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns("test");

        var sut = new ApplicationServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.UpdateApplicationAppointmentAsync(applicationId, appointmentDate, default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

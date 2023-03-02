using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Configuration;
using CCW.Application.Models;
using CCW.Application.Clients;
using FluentAssertions;
using System.Text.Json;

namespace CCW.Application.Tests;

public class AdminServiceClientTests
{
    [AutoMoqData]
    [Test]
    public async Task GetAgencyProfileSettingsAsync_ShouldReturn_AgencyProfileSettings(
        AgencyProfileSettingsModel responseModel
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
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
                    { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
            }).Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://google.com/")
        };

        var iConfigurationMock = new Mock<IConfiguration>();
        iConfigurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns("test");

        var sut = new AdminServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.GetAgencyProfileSettingsAsync(default);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(responseModel);
    }

    [AutoMoqData]
    [Test]
    public async Task GetAgencyProfileSettingsAsync_Throws_WhenRequest_Unsuccessful(
        AgencyProfileSettingsModel responseModel
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
                StatusCode = HttpStatusCode.BadGateway,
                Content = new StringContent(JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
                    { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
            }).Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://google.com/")
        };

        var iConfigurationMock = new Mock<IConfiguration>();
        iConfigurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns("test");

        var sut = new AdminServiceClient(httpClient, iConfigurationMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.GetAgencyProfileSettingsAsync(default)).Should()
            .ThrowAsync<Exception>();
    }
}

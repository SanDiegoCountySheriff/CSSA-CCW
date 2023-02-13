using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CCW.Application.Clients;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace CCW.Application.Tests;

public class DocumentServiceClientTests
{
    [AutoMoqData]
    [Test]
    public async Task GetApplicantImageAsync_ShouldReturn_ApplicantImage(
        string fileName
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

        var sut = new DocumentServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.GetApplicantImageAsync(fileName, default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [AutoMoqData]
    [Test]
    public async Task GetApplicationTemplateAsync_ShouldReturn_ApplicationTemplate()
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

        var sut = new DocumentServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.GetApplicationTemplateAsync(default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [AutoMoqData]
    [Test]
    public async Task GetOfficialLicenseTemplateAsync_ShouldReturn_OfficialTemplate()
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

        var sut = new DocumentServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.GetOfficialLicenseTemplateAsync(default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [AutoMoqData]
    [Test]
    public async Task GetUnofficialLicenseTemplateAsync_ShouldReturn_SherriffSignature()
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

        var sut = new DocumentServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.GetUnofficialLicenseTemplateAsync(default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [AutoMoqData]
    [Test]
    public async Task GetSheriffSignatureAsync_ShouldReturn_SherriffSignature()
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

        var sut = new DocumentServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.GetSheriffSignatureAsync(default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [AutoMoqData]
    [Test]
    public async Task SaveApplicationPdfAsync_ShouldReturn_Ok(IFormFile fileToUpload, string saveAsFileName)
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

        var sut = new DocumentServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.SaveApplicationPdfAsync(fileToUpload, saveAsFileName, cancellationToken:default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [AutoMoqData]
    [Test]
    public async Task SaveOfficialLicensePdfAsync_ShouldReturn_Ok(IFormFile fileToUpload, string saveAsFileName)
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

        var sut = new DocumentServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.SaveOfficialLicensePdfAsync(fileToUpload, saveAsFileName, cancellationToken: default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [AutoMoqData]
    [Test]
    public async Task SaveUnofficialLicensePdfAsync_ShouldReturn_Ok(IFormFile fileToUpload, string saveAsFileName)
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

        var sut = new DocumentServiceClient(httpClient, iConfigurationMock.Object);

        // Act
        var result = await sut.SaveUnofficialLicensePdfAsync(fileToUpload, saveAsFileName, cancellationToken: default);

        // Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
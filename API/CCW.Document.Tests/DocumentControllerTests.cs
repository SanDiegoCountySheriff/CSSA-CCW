using CCW.Document.Controllers;
using CCW.Document.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO.Compression;
using AutoFixture;
using Azure.Storage.Blobs.Models;
using NSubstitute;
using BlobProperties = Microsoft.WindowsAzure.Storage.Blob.BlobProperties;
using Azure;
using Azure.Core;
using Microsoft.WindowsAzure.Storage.Auth;
using NSubstitute.Core;

namespace CCW.Document.Tests;

internal class DocumentControllerTests
{
    protected Mock<IAzureStorage> _azureStorageMock { get; }
    protected Mock<ILogger<DocumentController>> _loggerMock { get; }

    public DocumentControllerTests()
    {
        _azureStorageMock = new Mock<IAzureStorage>();
        _loggerMock = new Mock<ILogger<DocumentController>>();
    }


    [AutoMoqData]
    [Test]
    public async Task UploadApplicantFile_ShouldReturn_Ok(
        IFormFile fileToPersist,
        string saveAsFileName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.UploadApplicantFileAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.UploadApplicantFile(fileToPersist, saveAsFileName, default);
        var okObjectResult = result as OkResult;

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadApplicantFile_Should_Throw_When_Error(
        IFormFile fileToPersist,
        string saveAsFileName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.UploadApplicantFileAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UploadApplicantFile(fileToPersist, saveAsFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to upload applicant file.");
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyLogo_ShouldReturn_Ok(
        IFormFile fileToPersist,
        string saveAsFileName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.UploadAgencyLogoAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.UploadAgencyLogo(fileToPersist, saveAsFileName, default);
        var okResult = result as OkResult;

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyLogo_Should_Throw_When_Error(
        IFormFile fileToPersist,
        string saveAsFileName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.UploadAgencyLogoAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UploadAgencyLogo(fileToPersist, saveAsFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to upload agency logo.");
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadApplicantFile_ShouldReturn_File(
        string applicantFileName,
        string expected
    )
    {
        // Arrange
        var stream = new Mock<MemoryStream>();

        var blob = new Mock<CloudBlockBlob>(new Uri("http://storageaccount/container/blob"));
        var cred = new StorageCredentials("account", Convert.ToBase64String(Encoding.Unicode.GetBytes("key")), "keyname");
        var containerMock = new Mock<CloudBlobContainer>(new Uri("http://storageaccount/container"), cred);
        blob.Setup(m => m.ExistsAsync())
            .ReturnsAsync(true);
        blob.Setup(m => m.DownloadToStreamAsync(It.IsAny<MemoryStream>()))
            .Returns(Task.FromResult(stream));
        blob.Setup(x => x.OpenReadAsync())
            .ReturnsAsync(stream.Object);
        blob.SetupAllProperties().Setup(x=>x.Name).Returns(expected);
        blob.Object.Properties.ContentType = "application/pdf";
        containerMock.Setup(c => c.GetBlockBlobReference(It.IsAny<string>()))
            .Returns(blob.Object);
        
        _azureStorageMock.Setup(x => x.DownloadApplicantFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(blob.Object);

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.DownloadApplicantFile(applicantFileName, default);
        var fileStreamResult = result as FileStreamResult;
        // Assert
        result.Should().BeOfType<FileStreamResult>();
        fileStreamResult?.FileDownloadName.Should().Be(expected);
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadApplicantFile_ShouldReturn_Content_Empty_IfNotExists(
        string applicantFileName,
        Stream blobStream
    )
    {
        // Arrange
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write("sample data");
        writer.Flush();
        stream.Position = 0;

        var blobMock = new Mock<CloudBlockBlob>(new Uri("http://tempuri.org/blob"));
        blobMock
            .Setup(m => m.ExistsAsync())
            .ReturnsAsync(false);
        blobMock
            .Setup(m => m.DownloadToStreamAsync(It.IsAny<MemoryStream>()))
            .Returns(Task.FromResult(stream));
        blobMock.Setup(x => x.OpenReadAsync()).ReturnsAsync(blobStream);

        _azureStorageMock.Setup(x => x.DownloadApplicantFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(blobMock.Object);

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.DownloadApplicantFile(applicantFileName, default);
        var contentResult = result as ContentResult;

        // Assert
        result.Should().BeOfType<ContentResult>();
        contentResult?.Content.Should().Be("Image does not exist");
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadApplicantFile_Should_Throw_When_Error(
        string applicantFileName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.DownloadApplicantFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DownloadApplicantFile(applicantFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to download applicant file.");
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyLogo_ShouldReturn_String(
        string agencyLogoName,
        string response
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.DownloadAgencyLogoAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.DownloadAgencyLogo(agencyLogoName, default);

        // Assert
        result.Should().BeOfType<string>();
        result.Should().Be(response);
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyLogo_Should_Throw_When_Error(
        string agencyLogoName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.DownloadAgencyLogoAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DownloadAgencyLogo(agencyLogoName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to download agency logo.");
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteAgencyLogo_ShouldReturn_String(
        string agencyLogoName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.DeleteAgencyLogoAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.DeleteAgencyLogo(agencyLogoName, default);
        var okResult = result as OkResult;

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteAgencyLogo_Should_Throw_When_Error(
        string agencyLogoName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.DeleteAgencyLogoAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DeleteAgencyLogo(agencyLogoName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to delete agency logo.");
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteApplicantFile_ShouldReturn_Ok(
        string applicantFileName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.DeleteApplicantFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.DeleteApplicantFile(applicantFileName, default);
        var okResult = result as OkResult;

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteApplicantFile_Should_Throw_When_Error(
        string applicantFileName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.DeleteApplicantFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DeleteApplicantFile(applicantFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to delete applicant file.");
    }
}
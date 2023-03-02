using CCW.Document.Controllers;
using CCW.Document.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Security.Claims;
using AutoFixture;
using Azure.Storage.Blobs.Models;

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
        string saveAsFileName
    )
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.ContentType).Returns("application/pdf");

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        _azureStorageMock.Setup(x => x.UploadApplicantFileAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.UploadApplicantFile(fileMock.Object, saveAsFileName, default);
        var okObjectResult = result as OkResult;

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadApplicantFile_Should_Throw_WhenMissingContentType(
        IFormFile fileToPersist,
        string saveAsFileName
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

        _azureStorageMock.Setup(x => x.UploadApplicantFileAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.UploadApplicantFile(fileToPersist, saveAsFileName, default);
        var objResult = result as ObjectResult;
        var obj = objResult.Value as ValidationProblemDetails;

        // Assert
        result.Should().BeOfType<ObjectResult>();
        obj.Detail.Should().Be("Content type missing or invalid.");
        
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
    public async Task UploadUserApplicantFile_ShouldReturn_Ok(
        string saveAsFileName
    )
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.ContentType).Returns("application/pdf");

        _azureStorageMock.Setup(x => x.UploadApplicantFileAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.UploadUserApplicantFile(fileMock.Object, saveAsFileName, default);
        var okObjectResult = result as OkResult;

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadUserApplicantFile_Should_Return_WhenMissingContentType(
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
        var result = await sut.UploadUserApplicantFile(fileToPersist, saveAsFileName, default);
        var objResult = result as ObjectResult;
        var obj = objResult.Value as ValidationProblemDetails;

        // Assert
        result.Should().BeOfType<ObjectResult>();
        obj.Detail.Should().Be("Content type missing or invalid.");

    }

    [AutoMoqData]
    [Test]
    public async Task UploadUserApplicantFile_Should_Throw_When_Error(
        string saveAsFileName
    )
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.ContentType).Returns("application/pdf");

        _azureStorageMock.Setup(x => x.UploadApplicantFileAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UploadUserApplicantFile(fileMock.Object, saveAsFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to upload user applicant file.");
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyFile_ShouldReturn_Ok(
        string saveAsFileName
    )
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.ContentType).Returns("application/pdf");

        _azureStorageMock.Setup(x => x.UploadAgencyFileAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.UploadAgencyFile(fileMock.Object, saveAsFileName, default);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyFile_Should_Return_WhenMissingContentType(
        IFormFile file,
        string saveAsFileName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.UploadAgencyFileAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.UploadAgencyFile(file, saveAsFileName, default);
        var objResult = result as ObjectResult;
        var obj = objResult.Value as ValidationProblemDetails;

        // Assert
        result.Should().BeOfType<ObjectResult>();
        obj.Detail.Should().Be("Content type missing or invalid.");
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyFile_Should_Throw_When_Error(
        string saveAsFileName
    )
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.ContentType).Returns("application/pdf");

        _azureStorageMock.Setup(x => x.UploadAgencyFileAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UploadAgencyFile(fileMock.Object, saveAsFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to upload agency file.");
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyLogo_ShouldReturn_Ok(
        IFormFile fileToPersist,
        string saveAsFileName
    )
    {
        // Arrange

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.ContentType).Returns("application/pdf");

        _azureStorageMock.Setup(x => x.UploadAgencyLogoAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.UploadAgencyLogo(fileMock.Object, saveAsFileName, default);
        var okResult = result as OkResult;

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyLogo_Should_Return_WhenMissingContentType(
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
        var objResult = result as ObjectResult;
        var obj = objResult.Value as ValidationProblemDetails;

        // Assert
        result.Should().BeOfType<ObjectResult>();
        obj.Detail.Should().Be("Content type missing or invalid.");
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyLogo_Should_Throw_When_Error(
        string saveAsFileName
    )
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.ContentType).Returns("application/pdf");

        _azureStorageMock.Setup(x => x.UploadAgencyLogoAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UploadAgencyLogo(fileMock.Object, saveAsFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to upload agency logo.");
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadApplicantFile_ShouldReturn_FileStream_WhenContentTypePdf(
        string applicantFileName,
        string expected
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

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

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.DownloadApplicantFile(applicantFileName, default);

        // Assert
        result.Should().BeOfType<FileStreamResult>();
        stream.Object.Dispose();
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadApplicantFile_ShouldReturn_ContentResultBase64_WhenContentTypeImage(
        string applicantFileName,
        string expected
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

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
        blob.SetupAllProperties().Setup(x => x.Name).Returns(expected);
        blob.Object.Properties.ContentType = "application/png";
        containerMock.Setup(c => c.GetBlockBlobReference(It.IsAny<string>()))
            .Returns(blob.Object);

        _azureStorageMock.Setup(x => x.DownloadApplicantFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(blob.Object);

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.DownloadApplicantFile(applicantFileName, default);

        // Assert
        result.Should().BeOfType<ContentResult>();
        stream.Object.Dispose();
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadApplicantFile_ShouldReturn_Content_Empty_IfNotExists(
        string applicantFileName,
        Stream blobStream
    )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

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

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.DownloadApplicantFile(applicantFileName, default);
        var contentResult = result as ContentResult;

        // Assert
        result.Should().BeOfType<ContentResult>();
        contentResult?.Content.Should().Be("File/image does not exist");
        stream.Dispose();
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
    public async Task DownloadUserApplicantFile_ShouldReturn_FileStream_WhenContentTypePdf(
        string applicantFileName,
        string expected
    )
    {
       // Arrange
       var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
       }, "TestAuthentication"));

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
        blob.SetupAllProperties().Setup(x => x.Name).Returns(expected);
        blob.Object.Properties.ContentType = "application/pdf";
        blob.Object.Properties.ContentDisposition = "inline";
        containerMock.Setup(c => c.GetBlockBlobReference(It.IsAny<string>()))
            .Returns(blob.Object);

        _azureStorageMock.Setup(x => x.DownloadApplicantFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(blob.Object);

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.DownloadUserApplicantFile(applicantFileName, default);

        // Assert
        result.Should().BeOfType<FileStreamResult>();
        stream.Object.Dispose();
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadUserApplicantFile_ShouldReturn_ContentResultBase64_WhenContentTypeImage(
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
        blob.SetupAllProperties().Setup(x => x.Name).Returns(expected);
        blob.Object.Properties.ContentType = "application/png";
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
        var result = await sut.DownloadUserApplicantFile(applicantFileName, default);

        // Assert
        result.Should().BeOfType<ContentResult>();
        stream.Object.Dispose();
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadUserApplicantFile_ShouldReturn_Content_Empty_IfNotExists(
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
        var result = await sut.DownloadUserApplicantFile(applicantFileName, default);
        var contentResult = result as ContentResult;

        // Assert
        result.Should().BeOfType<ContentResult>();
        contentResult?.Content.Should().Be("File/image does not exist"); 
        stream.Dispose();
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadUserApplicantFile_Should_Throw_When_Error(
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
        await sut.Invoking(async x => await x.DownloadUserApplicantFile(applicantFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to download user applicant file.");
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyFile_ShouldReturn_FileStream_WhenContentTypePdf(
        string expected,
        string agencyFileName
 )
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "1234-9874")
        }, "TestAuthentication"));

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
        blob.SetupAllProperties().Setup(x => x.Name).Returns(expected);
        blob.Object.Properties.ContentType = "application/pdf";
        containerMock.Setup(c => c.GetBlockBlobReference(It.IsAny<string>()))
            .Returns(blob.Object);

        _azureStorageMock.Setup(x => x.DownloadAgencyFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(blob.Object);

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        sut.ControllerContext = new ControllerContext();
        sut.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        // Act
        var result = await sut.DownloadAgencyFile(agencyFileName, default);

        // Assert
        result.Should().BeOfType<FileStreamResult>();
        stream.Object.Dispose();
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyFile_ShouldReturn_Content_Empty_IfNotExists(
        string agencyFileName,
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

        _azureStorageMock.Setup(x => x.DownloadAgencyFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(blobMock.Object);

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        // Act
        var result = await sut.DownloadAgencyFile(agencyFileName, default);
        var contentResult = result as ContentResult;

        // Assert
        result.Should().BeOfType<ContentResult>();
        contentResult?.Content.Should().Be("File does not exist");
        stream.Dispose();
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyFile_Should_Throw_When_Error(
        string agencyFileName
    )
    {
        // Arrange
        _azureStorageMock.Setup(x => x.DownloadAgencyFileAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var sut = new DocumentController(
            _azureStorageMock.Object,
            _loggerMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DownloadAgencyFile(agencyFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to download agency file.");
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
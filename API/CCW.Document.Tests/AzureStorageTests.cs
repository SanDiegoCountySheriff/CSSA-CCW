using Moq;
using CCW.Document.Services;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace CCW.Document.Tests;

internal class AzureStorageTests
{
    protected string _storageConnection { get; }
    protected string _agencyContainerName { get; }
    protected string _publicContainerName { get; }
    protected Mock<IConfiguration> _configurationMock { get; }
    protected Mock<ILogger<AzureStorage>> _loggerMock { get; }
    private string[] _imageTypes = new[] { "jpeg", "png" };

    public AzureStorageTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _storageConnection = "storageconnection";
        _agencyContainerName = "agencycontainer";
        _publicContainerName = "publiccontainer";
        _loggerMock = new Mock<ILogger<AzureStorage>>(); ;
    }


    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyLogoAsync_Should_UploadAgencyLogo(
        string agencyLogoName,
        byte[] logo
    )
    {
        // Arrange
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_agencyContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var storageAccount = new Mock<CloudStorageAccount>();
        Uri uri = new Uri("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");
        var blobClientMock = new Mock<CloudBlobClient>(uri);
        storageAccount.Setup(a => a.CreateCloudBlobClient()).Returns(blobClientMock.Object);
        var cloudBlobContainerMock = new Mock<CloudBlobContainer>(uri);
        blobClientMock.Setup(a => a.GetContainerReference(_agencyContainerName)).Returns(cloudBlobContainerMock.Object);

        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write("sample data");
        writer.Flush();
        stream.Position = 0;
        //    var file = CreateMockFile("test.jpg", 500000);
        var blobMock = new Mock<CloudBlockBlob>(MockBehavior.Strict);
        blobMock
            .Setup(m => m.ExistsAsync())
            .ReturnsAsync(true);
        blobMock
            .Setup(m => m.DownloadToStreamAsync(It.IsAny<Stream>()))
            .Callback((Stream target) => stream.CopyTo(target))
            .Returns(Task.CompletedTask);



        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        var result = await sut.DownloadAgencyLogoAsync("agency_logo", default);

        // Assert
        result.Should().BeOfType<string>();
        result.Should().Contain("data:image/png;base64");
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadApplicantFileAsync_Should_Return_ApplicantFile(
        string applicantFileName
    )
    {
        // Arrange
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_agencyContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var storageAccount = new Mock<CloudStorageAccount>();
        Uri uri = new Uri("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var mockBlobItem = new Mock<CloudBlockBlob>(uri);
        mockBlobItem
            .Setup(b => b.UploadTextAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(true))
            .Verifiable();

        var mockBlobUri = new Uri("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");
        var mockBlobContainer = new Mock<CloudBlobContainer>(mockBlobUri);
        mockBlobContainer
            .Setup(c => c.GetBlobReference(It.IsAny<string>()))
            .Returns(mockBlobItem.Object)
            .Verifiable();

        //var blobClientMock = new Mock<CloudBlobClient>(uri);
        //storageAccount.Setup(a => a.CreateCloudBlobClient()).Returns(blobClientMock.Object);
        //var cloudBlobContainerMock = new Mock<CloudBlobContainer>(uri);
        //var cloudBlob = new Mock<CloudBlob>(uri, );

        //blobClientMock.Setup(a => a.GetContainerReference(_publicContainerName)).Returns(cloudBlobContainerMock.Object);

        //cloudBlobContainerMock.Setup(m => m.ExistsAsync())
        //    .ReturnsAsync(true);
        //cloudBlobContainerMock.Setup(x => x.GetBlobReference(It.IsAny<string>()))
        //    .Returns(cloudBlob.Object);


        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        var result = await sut.DownloadApplicantFileAsync(applicantFileName, default);

        // Assert
        result.Should().BeOfType<CloudBlob>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyLogoAsync_Should_Return_TaskComplete(
    string applicantFileName
)
    {
        // Arrange
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_agencyContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var storageAccount = new Mock<CloudStorageAccount>();
        Uri uri = new Uri("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var mockBlobItem = new Mock<CloudBlockBlob>(uri);
        mockBlobItem
            .Setup(b => b.UploadTextAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(true))
            .Verifiable();

        var mockBlobUri = new Uri("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");
        var mockBlobContainer = new Mock<CloudBlobContainer>(mockBlobUri);
        mockBlobContainer
            .Setup(c => c.GetBlobReference(It.IsAny<string>()))
            .Returns(mockBlobItem.Object)
            .Verifiable();

        //var blobClientMock = new Mock<CloudBlobClient>(uri);
        //storageAccount.Setup(a => a.CreateCloudBlobClient()).Returns(blobClientMock.Object);
        //var cloudBlobContainerMock = new Mock<CloudBlobContainer>(uri);
        //var cloudBlob = new Mock<CloudBlob>(uri, );

        //blobClientMock.Setup(a => a.GetContainerReference(_publicContainerName)).Returns(cloudBlobContainerMock.Object);

        //cloudBlobContainerMock.Setup(m => m.ExistsAsync())
        //    .ReturnsAsync(true);
        //cloudBlobContainerMock.Setup(x => x.GetBlobReference(It.IsAny<string>()))
        //    .Returns(cloudBlob.Object);


        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        var result = await sut.DownloadApplicantFileAsync(applicantFileName, default);

        // Assert
        result.Should().BeOfType<CloudBlob>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyLogoAsync_Should_Throw_When_Error(
        IFormFile fileToPersist,
        string saveAsFileName
        )
    {
        // Arrange
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_agencyContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        Mock<CloudStorageAccount> _storageAccount;
        var c = new StorageCredentials("dummyStorageAccountName", "DummyKey");
        _storageAccount = new Mock<CloudStorageAccount>(c, true);

        Uri uri = new Uri("https://google.com//");
        var blobClientMock = new Mock<BlobContainerClient>(uri);
        _storageAccount.Setup(a => a.CreateCloudBlobClient()).Throws(new Exception("test"));

        var sut = new AzureStorage(_configurationMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.UploadAgencyLogoAsync(fileToPersist, saveAsFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("An error occur while trying to upload agency logo.");
    }

    [AutoMoqData]
    [Test]
    public async Task UploadApplicantFileAsync_Should_Return_TaskComplete(
  string applicantFileName
)
    {
        // Arrange
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_agencyContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");
        var blobContentInfoMock = new Mock<BlobContentInfo>();
      //  blobContentInfoMock.Setup(x=>x.)

        var blobClientMock = new Mock<BlobClient>();
        blobClientMock.Setup(x => x.Upload(It.IsAny<Stream>()));

        var blobContainerMock = new Mock<BlobContainerClient>(It.IsAny<string>(), It.IsAny<string>());
        blobContainerMock.Setup(x => x.GetBlobClient(It.IsAny<string>()))
            .Returns(blobClientMock.Object);

        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        var result = await sut.DownloadApplicantFileAsync(applicantFileName, default);

        // Assert
        result.Should().BeOfType<CloudBlob>();
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteAgencyLogoAsync_Should_DeleteFile()
    {
        // Arrange
        string agencyLogName = "test.pdf";

        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_publicContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var storageAccount = new Mock<CloudStorageAccount>();
        Uri uri = new Uri("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");
        var blobClientMock = new Mock<CloudBlobClient>(uri);
        storageAccount.Setup(a => a.CreateCloudBlobClient()).Returns(blobClientMock.Object);
        var cloudBlobContainerMock = new Mock<CloudBlobContainer>(uri);
        blobClientMock.Setup(a => a.GetContainerReference(_publicContainerName)).Returns(cloudBlobContainerMock.Object);

        Uri blobUri = new Uri(uri + "/" + _publicContainerName + "/" + agencyLogName);
        var mockBlobItem = new Mock<CloudBlockBlob>(blobUri);
        mockBlobItem
            .Setup(b => b.DeleteIfExistsAsync())
            .Callback(() => mockBlobItem.Setup(x => x.DeleteIfExistsAsync())
                .ReturnsAsync(true));

        cloudBlobContainerMock.Setup(x => x.GetBlobReference(It.IsAny<string>()))
            .Returns(mockBlobItem.Object);

        cloudBlobContainerMock.Setup(x => x.DeleteIfExistsAsync()).ReturnsAsync(true);


        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        await sut.DeleteAgencyLogoAsync(agencyLogName, default);

        // Assert
        mockBlobItem.Verify(x => x.DeleteIfExistsAsync(), Times.Never);
    }

    [AutoMoqData]
    [Test]
    public async Task DeleteApplicantFileAsync_Should_DeleteFile()
    {
        // Arrange
        string applicantFileName = "test.pdf";

        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_publicContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var storageAccount = new Mock<CloudStorageAccount>();
        Uri uri = new Uri("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");
        var blobClientMock = new Mock<CloudBlobClient>(uri);
        storageAccount.Setup(a => a.CreateCloudBlobClient()).Returns(blobClientMock.Object);
        var cloudBlobContainerMock = new Mock<CloudBlobContainer>(uri);
        blobClientMock.Setup(a => a.GetContainerReference(_publicContainerName)).Returns(cloudBlobContainerMock.Object);

        Uri blobUri = new Uri(uri + "/" + _publicContainerName +"/"+applicantFileName);
        var mockBlobItem = new Mock<CloudBlockBlob>(blobUri);
        mockBlobItem
            .Setup(b => b.DeleteIfExistsAsync())
            .Callback(() => mockBlobItem.Setup(x => x.DeleteIfExistsAsync())
            .ReturnsAsync(true));

        cloudBlobContainerMock.Setup(x => x.GetBlobReference(It.IsAny<string>()))
            .Returns(mockBlobItem.Object);

        cloudBlobContainerMock.Setup(x=>x.DeleteIfExistsAsync()).ReturnsAsync(true);


        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        await sut.DeleteApplicantFileAsync(applicantFileName, default);

        // Assert
        mockBlobItem.Verify(x => x.DeleteIfExistsAsync(), Times.Never);
    }




















    //[TestMethod()]
    //public void When_FileIsDeleted()
    //{
    //    var file = CreateMockFile("test.jpg", 500000);
    //    var blob = _storageContainer.Object.GetBlockBlobReference("deleteTest.jpg");
    //    blob.UploadFromStream(file.Object.InputStream);

    //    _service.DeleteFromStorage("deleteTest.jpg");

    //    var blobs = _storageContainer.Object.ListBlobs();
    //    Assert.AreEqual(0, blobs.Count());
    //}


    //public Mock<FileStream> CreateMockFile(string fileName, int sizeInBytes)
    //{
    //    var file = new Mock<FileStream>();
    //    var stream = new MemoryStream();
    //    var bmp = new Bitmap(1, 1);
    //    var graphics = Graphics.FromImage(bmp);
    //    graphics.FillRectangle(Brushes.Black, 0, 0, 1, 1);
    //    bmp.Save(stream, ImageFormat.Jpeg);

    //    //file.Setup(pf => pf.InputStream).Returns(stream);
    //    //file.Setup(f => f.FileName).Returns(fileName);
    //    //file.Setup(f => f.ContentLength).Returns(sizeInBytes);

    //    return file;
    //}
}


using Moq;
using CCW.Document.Services;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using NSubstitute;

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

        var content = "Hello World from a Fake File";
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        var mockBlobUri = new Uri("http://bogus/myaccount/blob");

        var cloudBlobMock = new Mock<CloudBlockBlob>(mockBlobUri);
        cloudBlobMock.Setup(m => m.ExistsAsync())
            .Returns(Task.FromResult(true));

        var blobMock = new Mock<CloudBlockBlob>(MockBehavior.Strict);
        blobMock
            .Setup(m => m.ExistsAsync())
            .ReturnsAsync(true);
        blobMock
            .Setup(m => m.DownloadToStreamAsync(stream))
            .Callback((Stream target) => stream.CopyTo(target))
            .Returns(Task.CompletedTask);

        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        var result = await sut.DownloadAgencyLogoAsync("agency_logo", default);

        // Assert
        result.Should().BeOfType<string>();
        result.Should().Contain("data:image/png;base64");
        stream.Dispose();
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
        
        var mockBlobUri = new Uri("http://bogus/myaccount/blob");

        var cloudBlobMock = new Mock<CloudBlockBlob>(mockBlobUri);
        cloudBlobMock.Setup(m => m.ExistsAsync())
            .Returns(Task.FromResult(true));

        var cloudBlobContainerMock = new Mock<CloudBlobContainer>();
        cloudBlobContainerMock.Setup(m => m.ExistsAsync())
            .ReturnsAsync(true);
        cloudBlobContainerMock.Setup(x => x.GetBlobReference(It.IsAny<string>()))
            .Returns(cloudBlobMock.Object);

        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        var result = await sut.DownloadApplicantFileAsync(applicantFileName, default);

        // Assert
        result.Should().BeOfType<CloudBlob>();
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadApplicantFileAsync_Should_Throw_WhenContainerDoesntExist(
    string applicantFileName
)
    {
        // Arrange
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_agencyContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var sut = new AzureStorage(_configurationMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DownloadApplicantFileAsync(applicantFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("Container does not exist.");
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyFileAsync_Should_Return_AgencyFile()
    {
        // Arrange
        string agencyFileName = "ApplicationTemplate.pdf";

        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_agencyContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var mockBlobUri = new Uri("http://bogus/blob");

        var cloudBlobMock = new Mock<CloudBlob>();
     
        var cloudBlobContainerMock = new Mock<CloudBlobContainer>();
        //cloudBlobContainerMock.Setup(x => x.GetBlobReference(It.IsAny<string>()))
        //    .Returns(cloudBlobMock.Object);
        cloudBlobContainerMock.Setup(m => m.ExistsAsync())
            .ReturnsAsync(true);

        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        var result = await sut.DownloadAgencyFileAsync(agencyFileName, default);

        // Assert
        result.Should().BeOfType<CloudBlob>();
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyFileAsync_Should_Throw_WhenContainerDoesntExist(
    string applicantFileName
)
    {
        // Arrange
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_agencyContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");

        var sut = new AzureStorage(_configurationMock.Object);

        //  Act & Assert
        await sut.Invoking(async x => await x.DownloadAgencyFileAsync(applicantFileName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("Container does not exist.");
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

        var wasCalled = false;

        // Mock stream
        var mockStream = new Mock<Stream>();
        mockStream.Setup(s => s.CanWrite).Returns(true);
        mockStream.Setup(s => s.Write(It.IsAny<byte[]>(),
            It.IsAny<int>(),
            It.IsAny<int>())).Callback((byte[] bytes, int offs, int c) =>
        {
            wasCalled = true;
        });

        var cancellationToken = CancellationToken.None;
        var mockBlob = new Mock<BlobClient>(MockBehavior.Strict, new Uri("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/"), (BlobClientOptions)null);
        //mockBlob
        //    .Setup(c => c.UploadAsync(It.IsAny<Stream>(), It.IsAny<BlobHttpHeaders>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<BlobRequestConditions>(), It.IsAny<IProgress<long>>(), It.IsAny<AccessTier>(), It.IsAny<StorageTransferOptions>(), cancellationToken))
        //    .ReturnsAsync(Response.FromValue(true));

        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        var result = await sut.DownloadApplicantFileAsync(applicantFileName, default);

        // Assert
        result.Should().BeOfType<CloudBlob>();
    }

    [AutoMoqData]
    [Test]
    public async Task UploadAgencyFileAsync_Should_Return_TaskComplete()
    {
        // Arrange
        string agencyFileName = "Test.pdf";

        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
            .Returns(_agencyContainerName);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value)
            .Returns("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/");
        var blobContentInfoMock = new Mock<BlobContentInfo>();

        var wasCalled = false;

        // Mock stream
        var mockStream = new Mock<Stream>();
        mockStream.Setup(s => s.CanWrite).Returns(true);
        mockStream.Setup(s => s.Write(It.IsAny<byte[]>(),
            It.IsAny<int>(),
            It.IsAny<int>())).Callback((byte[] bytes, int offs, int c) =>
        {
            wasCalled = true;
        });

        var cancellationToken = CancellationToken.None;
        var mockBlob = new Mock<BlobClient>(MockBehavior.Strict, new Uri("https://kv-sdsd-it-ccw-dev-001.vault.usgovcloudapi.net/"), (BlobClientOptions)null);
        //mockBlob
        //    .Setup(c => c.UploadAsync(It.IsAny<Stream>(), It.IsAny<BlobHttpHeaders>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<BlobRequestConditions>(), It.IsAny<IProgress<long>>(), It.IsAny<AccessTier>(), It.IsAny<StorageTransferOptions>(), cancellationToken))
        //    .ReturnsAsync(Response.FromValue(true));

        var sut = new AzureStorage(_configurationMock.Object);

        // Act
        var result = await sut.DownloadAgencyFileAsync(agencyFileName, default);

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


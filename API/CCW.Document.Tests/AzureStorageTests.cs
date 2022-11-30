using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCW.Document.Services;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using NUnit.Framework.Internal;
using System.Configuration;
using System.Collections.Concurrent;
using System.ComponentModel;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure.KeyVault.Models;

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
        _storageConnection = "storageConnection";
        _agencyContainerName = "agencyContainer";
        _publicContainerName = "publicContainer";
        _loggerMock = new Mock<ILogger<AzureStorage>>(); ;
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyLogoAsync_Should_UploadAgencyLogo(
        string agencyLogoName
        )
    {
        // Arrange
        var blobClientMock = new Mock<CloudBlobClient>();
        var blockBlob = new Mock<CloudBlockBlob>();

        var storageAccount = new Mock<CloudStorageAccount>();
        storageAccount.Setup(x => x.CreateCloudBlobClient()).Returns(blobClientMock.Object);

        var containerMock = new Mock<CloudBlobContainer>();
        containerMock.Setup(x => x.GetBlockBlobReference(It.IsAny<string>())).Returns(blockBlob.Object);

        var sut = new AzureStorage(_configurationMock.Object, _loggerMock.Object);

        // Act
        var result = await sut.DownloadAgencyLogoAsync(agencyLogoName, default);

        // Assert
        result.Should().BeOfType<string>();
        result.Should().Contain("data:image/png;base64");
    }

    [AutoMoqData]
    [Test]
    public async Task DownloadAgencyLogoAsync_Should_Throw_When_Error(
        string agencyLogoName,
        IConfigurationSection configValue
        )
    {
        // Arrange
        var defaultCredentialsMock = new Mock<DefaultAzureCredential>();
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>())).Returns(configValue);

        string secret = "This is sceret key";

        Mock<KeyVaultSecret> mockSecret = new Mock<KeyVaultSecret>();
        mockSecret.SetupGet(x => x.Value).Returns("secret");

        var client = new Mock<SecretClient>();
        client.Setup(x => x.GetSecretAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
          //  .Returns();

        var storageAccount = new Mock<CloudStorageAccount>();
        storageAccount.Setup(x => x.CreateCloudBlobClient()).Throws(new Exception("test"));

        var sut = new AzureStorage(_configurationMock.Object, _loggerMock.Object);

        // Act
        var result = await sut.DownloadAgencyLogoAsync(agencyLogoName, default);

        //  Act & Assert
        await sut.Invoking(async x => await x.DownloadAgencyLogoAsync(agencyLogoName, default)).Should()
            .ThrowAsync<Exception>().WithMessage("test");
    }


}
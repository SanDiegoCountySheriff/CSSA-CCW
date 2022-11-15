using System.Reflection.Metadata.Ecma335;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CCW.Document.Services;

public class AzureStorage : IAzureStorage
{
    private readonly string _storageConnection;
    private readonly string _agencyContainerName;
    private readonly string _publicContainerName;
    private readonly ILogger<AzureStorage> _logger;

    private string[] _imageTypes = new[] { "jpeg", "png" };

    public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
    {
        var client = new SecretClient(new Uri(configuration.GetSection("KeyVault:VaultUri").Value),
            credential: new DefaultAzureCredential());
        _storageConnection = client.GetSecret("storage-connection-primary").Value.Value;
        _agencyContainerName = configuration.GetSection("Storage").GetSection("AgencyContainerName").Value;
        _publicContainerName = configuration.GetSection("Storage").GetSection("PublicContainerName").Value;
        _logger = logger;
    }

    public async Task<string> DownloadAgencyLogoAsync(string agencyLogoName, CancellationToken cancellationToken)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = BlobClient.GetContainerReference(_agencyContainerName);
        CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(agencyLogoName);

        string datauri;
        using (var memoryStream = new MemoryStream())
        {
            await blockBlob2.DownloadToStreamAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            var b64String = Convert.ToBase64String(bytes);
            datauri = "data:image/png;base64," + b64String;
        }

        return datauri;
    }

    public async Task<CloudBlob> DownloadApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer c1 = BlobClient.GetContainerReference(_publicContainerName);

        if (await c1.ExistsAsync())
        {
            CloudBlob file = c1.GetBlobReference(applicantFileName);

            return file;
        }

        throw new Exception("Container does not exist");
    }

    public Task UploadAgencyLogoAsync(IFormFile fileToPersist, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);

        try
        {
            var fileType = saveAsFileName.Substring(saveAsFileName.LastIndexOf('.') + 1);
            var contentType = _imageTypes.Contains(fileType) ? "image" : "application";

            BlobClient blob = container.GetBlobClient(saveAsFileName);

            using (Stream file = fileToPersist.OpenReadStream())
            {
                blob.Upload(file, new BlobHttpHeaders { ContentType = contentType + "/" + fileType });
            }
        }
        catch
        {
            // log error
        }

        return Task.CompletedTask;
    }

    public Task UploadApplicantFileAsync(IFormFile fileToPersist, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName);

        try
        {
            var fileType = saveAsFileName.Substring(saveAsFileName.LastIndexOf('.') + 1);
            var contentType = _imageTypes.Contains(fileType) ? "image" : "application";

            BlobClient blob = container.GetBlobClient(saveAsFileName);

            using (Stream file = fileToPersist.OpenReadStream())
            {
                blob.Upload(file, new BlobHttpHeaders { ContentType = contentType + "/" + fileType });
            }
        }
        catch
        {
            // log error
        }

        return Task.CompletedTask;
    }

    public async Task DeleteAgencyLogoAsync(string agencyLogoName, CancellationToken cancellationToken)
    {
        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_agencyContainerName);

        var blob = cloudBlobContainer.GetBlobReference(agencyLogoName);
        await blob.DeleteIfExistsAsync();
    }

    public async Task DeleteApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken)
    {
        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_publicContainerName);

        var blob = cloudBlobContainer.GetBlobReference(applicantFileName);
        await blob.DeleteIfExistsAsync();
    }
}

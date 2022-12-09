using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace CCW.Document.Services;

public class AzureStorage : IAzureStorage
{
    private readonly string _storageConnection;
    private readonly string _agencyContainerName;
    private readonly string _publicContainerName;

    private string[] _imageTypes = new[] { "jpeg", "png" };

    public AzureStorage(IConfiguration configuration)
    {
        var client = new SecretClient(new Uri(configuration.GetSection("KeyVault:VaultUri").Value),
            credential: new DefaultAzureCredential());
        _storageConnection = client.GetSecret("storage-connection-primary").Value.Value;
        _agencyContainerName = configuration.GetSection("Storage").GetSection("AgencyContainerName").Value;
        _publicContainerName = configuration.GetSection("Storage").GetSection("PublicContainerName").Value;
    }

    public async Task<string> DownloadAgencyLogoAsync(string agencyLogoName, CancellationToken cancellationToken)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference(_agencyContainerName);
        CloudBlockBlob blockBlob = container.GetBlockBlobReference(agencyLogoName);

        string datauri;
        using (var memoryStream = new MemoryStream())
        {
            await blockBlob.DownloadToStreamAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            var b64String = Convert.ToBase64String(bytes);
            datauri = "data:image/png;base64," + b64String;
        }

        return datauri;
    }

    public async Task<CloudBlob> DownloadApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(_publicContainerName);

        if (await cloudBlobContainer.ExistsAsync())
        {
            CloudBlob file = cloudBlobContainer.GetBlobReference(applicantFileName);

            return file;
        }

        throw new Exception("Container does not exist.");

    }

    public Task UploadAgencyLogoAsync(IFormFile fileToPersist, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);

        var fileType = saveAsFileName.Substring(saveAsFileName.LastIndexOf('.') + 1);
        var contentType = _imageTypes.Contains(fileType) ? "image" : "application";

        BlobClient blob = container.GetBlobClient(saveAsFileName);

        using (Stream file = fileToPersist.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = contentType + "/" + fileType });
        }

        return Task.CompletedTask;
    }

    public Task UploadApplicantFileAsync(IFormFile fileToPersist, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName);

        var fileType = saveAsFileName.Substring(saveAsFileName.LastIndexOf('.') + 1);
        var contentType = _imageTypes.Contains(fileType) ? "image" : "application";
        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = container.GetBlobClient(encodedName);

        using (Stream file = fileToPersist.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = contentType + "/" + fileType });
        }

        return Task.CompletedTask;
    }

    public Task UploadAgencyFileAsync(IFormFile fileToPersist, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);

        var fileType = saveAsFileName.Substring(saveAsFileName.LastIndexOf('.') + 1);
        var contentType = _imageTypes.Contains(fileType) ? "image" : "application";
        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = container.GetBlobClient(encodedName);

        using (Stream file = fileToPersist.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = contentType + "/" + fileType });
        }

        return Task.CompletedTask;
    }

    public async Task<CloudBlob> DownloadAgencyFileAsync(string agencyFileName, CancellationToken cancellationToken)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(_agencyContainerName);

        if (await cloudBlobContainer.ExistsAsync())
        {
            CloudBlob file = cloudBlobContainer.GetBlobReference(agencyFileName);

            return file;
        }

        throw new Exception("Container does not exist.");

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
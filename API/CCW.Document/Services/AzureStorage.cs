using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;

namespace CCW.Document.Services;

public class AzureStorage : IAzureStorage
{
    private readonly string _storageConnection;
    private readonly string _agencyContainerName;
    private readonly string _publicContainerName;
    private readonly string _adminUserContainerName;
    private readonly string _adminApplicationContainerName;
    private readonly BlobClientOptions _blobClientOptions;
    public AzureStorage(IConfiguration configuration)
    {
        var client = new SecretClient(new Uri(configuration.GetSection("KeyVault:VaultUri").Value),
            credential: new DefaultAzureCredential());
#if DEBUG
        _storageConnection = configuration.GetSection("AzureStorageEmulator:DocumentStorageConnection").Value;
#else
        _storageConnection = client.GetSecret("storage-ct-connection-primary").Value.Value;
#endif
        _agencyContainerName = configuration.GetSection("Storage").GetSection("AgencyContainerName").Value;
        _publicContainerName = configuration.GetSection("Storage").GetSection("PublicContainerName").Value;
        _adminUserContainerName = configuration.GetSection("Storage").GetSection("AdminUserContainerName").Value;
        _adminApplicationContainerName = configuration.GetSection("Storage").GetSection("AdminApplicationContainerName").Value;
        var handler = new HttpClientHandler()
        {
            Proxy = new WebProxy()
            {
                BypassProxyOnLocal = true
            }
        };
        _blobClientOptions = new BlobClientOptions()
        {
            Transport = new HttpClientTransport(handler)
        };
    }

    public async Task<string> DownloadAgencyLogoAsync(string agencyLogoName, CancellationToken cancellationToken)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference(_agencyContainerName);
        CloudBlockBlob blockBlob = container.GetBlockBlobReference(agencyLogoName);

        string datauri = string.Empty;
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

    public Task UploadAgencyLogoAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);

        BlobClient blob = container.GetBlobClient(saveAsFileName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType });
        }

        return Task.CompletedTask;
    }

    public Task UploadApplicantFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName);

        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = container.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType });
        }

        return Task.CompletedTask;
    }

    public Task UploadAdminApplicationFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container;
        container = new BlobContainerClient(_storageConnection, _adminApplicationContainerName);
#if DEBUG
        container = new BlobContainerClient("UseDevelopmentStorage=true", _adminApplicationContainerName);
#endif
        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = container.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType });
        }

        return Task.CompletedTask;
    }

    public async Task UploadAdminUserFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminUserContainerName, _blobClientOptions);
        await container.CreateIfNotExistsAsync();
        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = container.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType });
        }
    }

    public Task UploadAgencyFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);

        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = container.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType });
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

    public async Task DeleteAdminApplicationFileAsync(string adminApplicationFileName, CancellationToken cancellationToken)
    {
        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_adminApplicationContainerName);

        var blob = cloudBlobContainer.GetBlobReference(adminApplicationFileName);
        await blob.DeleteIfExistsAsync();
    }

    public async Task<CloudBlob> DownloadAdminUserFileAsync(string adminUserFileName, CancellationToken cancellationToken)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(_adminUserContainerName);

        if (await cloudBlobContainer.ExistsAsync())
        {
            CloudBlob file = cloudBlobContainer.GetBlobReference(adminUserFileName);

            return file;
        }

        throw new Exception("Container does not exist.");
    }

    public async Task<CloudBlob> DownloadAdminApplicationFileAsync(string adminApplicationFileName, CancellationToken cancellationToken)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageConnection);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(_adminApplicationContainerName);

        if (await cloudBlobContainer.ExistsAsync())
        {
            CloudBlob file = cloudBlobContainer.GetBlobReference(adminApplicationFileName);

            return file;
        }

        throw new Exception("Container does not exist.");
    }
}
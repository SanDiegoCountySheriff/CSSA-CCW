using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
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
        _storageConnection = configuration.GetSection("Storage").GetSection("LocalConnectionString").Value;
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
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
        await container.CreateIfNotExistsAsync();
        BlockBlobClient blockBlob = container.GetBlockBlobClient(agencyLogoName);

        string datauri = string.Empty;
        using (var memoryStream = new MemoryStream())
        {
            await blockBlob.DownloadToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            var b64String = Convert.ToBase64String(bytes);
            datauri = "data:image/png;base64," + b64String;
        }

        return datauri;
    }

    public async Task<BlobClient> DownloadApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName, _blobClientOptions);
        await container.CreateIfNotExistsAsync();

        if (await container.ExistsAsync())
        {
            BlobClient file = container.GetBlobClient(applicantFileName);

            return file;
        }

        throw new Exception("Container does not exist.");

    }

    public async Task UploadAgencyLogoAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
        await container.CreateIfNotExistsAsync();

        BlobClient blob = container.GetBlobClient(saveAsFileName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType });
        }
    }

    public async Task UploadApplicantFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName, _blobClientOptions);
        await container.CreateIfNotExistsAsync();

        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = container.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType });
        }
    }

    public async Task UploadAdminApplicationFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminApplicationContainerName, _blobClientOptions);
        await container.CreateIfNotExistsAsync();
        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = container.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType });
        }
    }

    public async Task UpdateAdminApplicationFileNameAsync(string oldName, string newName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminApplicationContainerName, _blobClientOptions);

        if (await container.ExistsAsync())
        {
            BlobClient file = container.GetBlobClient(oldName);

            if (await file.ExistsAsync())
            {
                BlobClient newFile = container.GetBlobClient(newName);

                await newFile.StartCopyFromUriAsync(file.Uri);
                await file.DeleteIfExistsAsync();
            }
            else
            {
                throw new Exception("File does not exist.");
            }
        }
        else
        {
            throw new Exception("Container does not exist.");
        }
    }

    public async Task UpdateApplicationFileNameAsync(string oldName, string newName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName, _blobClientOptions);

        if (await container.ExistsAsync())
        {
            BlobClient file = container.GetBlobClient(oldName);

            if (await file.ExistsAsync())
            {
                BlobClient newFile = container.GetBlobClient(newName);

                await newFile.StartCopyFromUriAsync(file.Uri);
                await file.DeleteIfExistsAsync();
            }
            else
            {
                throw new Exception("File does not exist.");
            }
        }
        else
        {
            throw new Exception("Container does not exist.");
        }
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

    public async Task UploadAgencyFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
        await container.CreateIfNotExistsAsync();

        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = container.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType });
        }
    }

    public async Task<BlobClient> DownloadAgencyFileAsync(string agencyFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
        await container.CreateIfNotExistsAsync();

        if (await container.ExistsAsync())
        {
            BlobClient file = container.GetBlobClient(agencyFileName);

            return file;
        }

        throw new Exception("Container does not exist.");

    }

    public async Task DeleteAgencyLogoAsync(string agencyLogoName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
        var blob = container.GetBlobClient(agencyLogoName);
        await blob.DeleteIfExistsAsync();
    }

    public async Task DeleteApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName, _blobClientOptions);
        if (await container.ExistsAsync())
        {
            BlobClient file = container.GetBlobClient(applicantFileName);
            await file.DeleteIfExistsAsync();
        }
    }

    public async Task DeleteAdminApplicationFileAsync(string adminApplicationFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminApplicationContainerName, _blobClientOptions);
        if (await container.ExistsAsync())
        {
            BlobClient file = container.GetBlobClient(adminApplicationFileName);
            await file.DeleteIfExistsAsync();
        }
    }

    public async Task<BlobClient> DownloadAdminUserFileAsync(string adminUserFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminUserContainerName, _blobClientOptions);
        if (await container.ExistsAsync())
        {
            BlobClient file = container.GetBlobClient(adminUserFileName);

            return file;
        }

        throw new Exception("Container does not exist.");
    }

    public async Task<BlobClient> DownloadAdminApplicationFileAsync(string adminApplicationFileName, CancellationToken cancellationToken)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminApplicationContainerName, _blobClientOptions);
        if (await container.ExistsAsync())
        {
            BlobClient file = container.GetBlobClient(adminApplicationFileName);

            return file;
        }

        throw new Exception("Container does not exist.");
    }
}
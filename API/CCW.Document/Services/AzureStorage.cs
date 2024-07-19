using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using CCW.Document.Services.Contracts;

namespace CCW.Document.Services;

public class AzureStorage : IAzureStorage
{
    private readonly BlobContainerClient _agencyContainer;
    private readonly BlobContainerClient _publicContainer;
    private readonly BlobContainerClient _adminUserContainer;
    private readonly BlobContainerClient _adminApplicationContainer;
    private readonly IStorageContainerResolver _storageContainerResolver;
    private readonly IHttpContextAccessor _contextAccessor;

    public AzureStorage(IStorageContainerResolver storageContainerResolver, IHttpContextAccessor contextAccessor)
    {
        _storageContainerResolver = storageContainerResolver;
        _contextAccessor = contextAccessor;

        var tenantId = _contextAccessor.HttpContext.Items["TenantId"] != null ? _contextAccessor.HttpContext.Items["TenantId"].ToString() : "";

        if (!string.IsNullOrEmpty(tenantId))
        {
            _agencyContainer = _storageContainerResolver.GetBlobContainer(tenantId, "ccw-agency-documents");
            _publicContainer = _storageContainerResolver.GetBlobContainer(tenantId, "ccw-public-documents");
            _adminUserContainer = _storageContainerResolver.GetBlobContainer(tenantId, "ccw-admin-user-documents");
            _adminApplicationContainer = _storageContainerResolver.GetBlobContainer(tenantId, "ccw-admin-application-documents");
        }
    }

    public async Task<string> DownloadAgencyLogoAsync(string tenantId, string agencyLogoName, CancellationToken cancellationToken)
    {
        var container = _storageContainerResolver.GetBlobContainer(tenantId, "ccw-agency-documents");
        BlockBlobClient blockBlob = container.GetBlockBlobClient(agencyLogoName);

        string datauri = string.Empty;
        using (var memoryStream = new MemoryStream())
        {
            await blockBlob.DownloadToAsync(memoryStream, cancellationToken);
            var bytes = memoryStream.ToArray();
            var b64String = Convert.ToBase64String(bytes);
            datauri = "data:image/png;base64," + b64String;
        }

        return datauri;
    }

    public async Task<BlobClient> DownloadApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken)
    {
        return _publicContainer.GetBlobClient(applicantFileName);
    }

    public async Task UploadAgencyLogoAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        BlobClient blob = _agencyContainer.GetBlobClient(saveAsFileName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType }, cancellationToken: cancellationToken);
        }
    }

    public async Task UploadApplicantFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = _publicContainer.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType }, cancellationToken: cancellationToken);
        }
    }

    public async Task UpdateAdminApplicationFileNameAsync(string oldName, string newName, CancellationToken cancellationToken)
    {
        BlobClient file = _adminApplicationContainer.GetBlobClient(oldName);

        if (await file.ExistsAsync(cancellationToken))
        {
            BlobClient newFile = _adminApplicationContainer.GetBlobClient(newName);

            await newFile.StartCopyFromUriAsync(file.Uri, cancellationToken: cancellationToken);
            await file.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }
        else
        {
            throw new Exception("File does not exist.");
        }
    }

    public async Task UpdateApplicationFileNameAsync(string oldName, string newName, CancellationToken cancellationToken)
    {
        BlobClient file = _publicContainer.GetBlobClient(oldName);

        if (await file.ExistsAsync(cancellationToken))
        {
            BlobClient newFile = _publicContainer.GetBlobClient(newName);

            await newFile.StartCopyFromUriAsync(file.Uri, cancellationToken: cancellationToken);
            await file.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }
        else
        {
            throw new Exception("File does not exist.");
        }
    }

    public async Task UploadAdminUserFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = _adminUserContainer.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType }, cancellationToken: cancellationToken);
        }
    }

    public async Task UploadAgencyFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var encodedName = System.Web.HttpUtility.UrlEncode(saveAsFileName);

        BlobClient blob = _agencyContainer.GetBlobClient(encodedName);

        using (Stream file = fileToUpload.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToUpload.ContentType }, cancellationToken: cancellationToken);
        }
    }

    public async Task<BlobClient> DownloadAgencyFileAsync(string agencyFileName, CancellationToken cancellationToken)
    {
        return _agencyContainer.GetBlobClient(agencyFileName);
    }

    public async Task<bool> ValidateAgencyFileAsync(string agencyFileName, CancellationToken cancellationToken)
    {
        BlobClient file = _agencyContainer.GetBlobClient(agencyFileName);

        if (await file.ExistsAsync(cancellationToken))
        {
            return true;
        }

        return false;
    }

    public async Task DeleteAgencyLogoAsync(string agencyLogoName, CancellationToken cancellationToken)
    {
        var blob = _agencyContainer.GetBlobClient(agencyLogoName);
        await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task DeleteApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken)
    {
        BlobClient file = _publicContainer.GetBlobClient(applicantFileName);
        await file.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task DeleteApplicantFilePublicAsync(string applicantFileName, CancellationToken cancellationToken)
    {
        BlobClient file = _publicContainer.GetBlobClient(applicantFileName);
        await file.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task DeleteAdminApplicationFileAsync(string adminApplicationFileName, CancellationToken cancellationToken)
    {
        BlobClient file = _adminApplicationContainer.GetBlobClient(adminApplicationFileName);
        await file.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<BlobClient> DownloadAdminUserFileAsync(string adminUserFileName, CancellationToken cancellationToken)
    {
        return _adminUserContainer.GetBlobClient(adminUserFileName);
    }

    public async Task<BlobClient> DownloadAdminApplicationFileAsync(string adminApplicationFileName, CancellationToken cancellationToken)
    {
        return _adminApplicationContainer.GetBlobClient(adminApplicationFileName);
    }
}
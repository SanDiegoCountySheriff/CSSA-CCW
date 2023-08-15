using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CCW.Document.Services;

public interface IAzureStorage
{
    Task UploadApplicantFileAsync(IFormFile file, string saveAsFileName, CancellationToken cancellationToken);

    Task<BlobClient> DownloadApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken);

    Task UploadAgencyFileAsync(IFormFile file, string saveAsFileName, CancellationToken cancellationToken);

    Task<BlobClient> DownloadAgencyFileAsync(string agencyFileName, CancellationToken cancellationToken);

    Task UploadAgencyLogoAsync(IFormFile file, string saveAsFileName, CancellationToken cancellationToken);

    Task<string> DownloadAgencyLogoAsync(string agencyLogoName, CancellationToken cancellationToken);

    Task DeleteAgencyLogoAsync(string agencyLogoName, CancellationToken cancellationToken);

    Task DeleteApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken);
    Task UploadAdminUserFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken);
    Task<BlobClient> DownloadAdminUserFileAsync(string adminUserFileName, CancellationToken cancellationToken);
    Task DeleteAdminApplicationFileAsync(string adminApplicationFileName, CancellationToken cancellationToken);
    Task UploadAdminApplicationFileAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken);
    Task<BlobClient> DownloadAdminApplicationFileAsync(string adminApplicationFileName, CancellationToken cancellationToken);
    Task UpdateAdminApplicationFileNameAsync(string oldName, string newName, CancellationToken cancellationToken);
    Task UpdateApplicationFileNameAsync(string oldName, string newName, CancellationToken cancellationToken);
}
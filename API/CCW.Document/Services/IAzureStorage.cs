using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CCW.Document.Services;

public interface IAzureStorage
{
    Task UploadApplicantFileAsync(IFormFile file, string saveAsFileName, CancellationToken cancellationToken);

    Task<CloudBlob> DownloadApplicantFileAsync(string applicantFileName, CancellationToken cancellationToken);

    Task UploadAgencyLogoAsync(IFormFile file, string saveAsFileName, CancellationToken cancellationToken);

    Task<string> DownloadAgencyLogoAsync(string agencyLogoName, CancellationToken cancellationToken);

    Task DeleteAsync(string blobFilename, CancellationToken cancellationToken);
}
using Azure.Storage.Blobs;

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
}
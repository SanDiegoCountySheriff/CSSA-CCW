using Microsoft.AspNetCore.Mvc;

namespace CCW.Application.Clients;

public interface IDocumentServiceClient
{
    Task<HttpResponseMessage> GetApplicantImageAsync(string fileName, CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetApplicationTemplateAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetUnofficialLicenseTemplateAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetOfficialLicenseTemplateAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetLiveScanTemplateAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetSheriffSignatureAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetSheriffLogoAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetProcessorSignatureAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> SaveAdminApplicationPdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken);
}
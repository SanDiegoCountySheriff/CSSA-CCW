using Microsoft.AspNetCore.Mvc;

namespace CCW.Application.Clients;

public interface IDocumentServiceClient
{
    Task<HttpResponseMessage> GetApplicantImageAsync(string fileName, CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetApplicationTemplateAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetUnofficialLicenseTemplateAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetOfficialLicenseTemplateAsync(CancellationToken cancellationToken);
    Task<HttpResponseMessage> SaveApplicationPdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken);
    Task<HttpResponseMessage> SaveUnofficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken);
    Task<HttpResponseMessage> SaveOfficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken);
}
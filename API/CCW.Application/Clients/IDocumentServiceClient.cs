using Microsoft.AspNetCore.Mvc;

namespace CCW.Application.Clients;

public interface IDocumentServiceClient
{
    Task<HttpResponseMessage> GetApplicationTemplateAsync(string templateName, CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetUnofficialLicenseTemplateAsync(string templateName, CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetOfficialLicenseTemplateAsync(string templateName, CancellationToken cancellationToken);
    Task<HttpResponseMessage> SaveApplicationPdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken);
    Task<HttpResponseMessage> SaveUnofficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken);
    Task<HttpResponseMessage> SaveOfficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken);
}
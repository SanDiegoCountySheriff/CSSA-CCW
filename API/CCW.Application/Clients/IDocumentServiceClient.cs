using Microsoft.AspNetCore.Mvc;

namespace CCW.Application.Clients;

public interface IDocumentServiceClient
{
    Task<FileStreamResult> GetApplicationTemplateAsync(string templateName, CancellationToken cancellationToken);
}
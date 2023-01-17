using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;

namespace CCW.Application.Clients;

public class DocumentServiceClient : IDocumentServiceClient
{
    private readonly HttpClient _httpClient;

    public DocumentServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FileStreamResult> GetApplicationTemplateAsync(string templateName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "downloadAgencyFile/"+ templateName);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return null!;
        
      //  return result.Content.ReadAsStreamAsync(cancellationToken:default);

    }

}
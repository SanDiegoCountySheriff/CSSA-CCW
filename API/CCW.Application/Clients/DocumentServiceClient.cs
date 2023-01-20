using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace CCW.Application.Clients;

public class DocumentServiceClient : IDocumentServiceClient
{
    private readonly HttpClient _httpClient;

    public DocumentServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetApplicationTemplateAsync(string templateName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "Document/v1/Document/downloadAgencyFile?agencyFileName=" + templateName);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> GetOfficialLicenseTemplateAsync(string templateName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "Document/v1/Document/downloadAgencyFile?agencyFileName=" + templateName);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> GetUnofficialLicenseTemplateAsync(string templateName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "Document/v1/Document/downloadAgencyFile?agencyFileName=" + templateName);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> SaveApplicationPdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "Document/v1/Document/uploadAgencyFile?saveAsFileName=" + saveAsFileName);
        var form = new MultipartFormDataContent();

        var streamContent = new StreamContent(fileToUpload.OpenReadStream());

        var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());

        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

        // "file" parameter name should be the same as the server side input parameter name
        form.Add(fileContent, "fileToUpload", saveAsFileName);
        request.Content = form;
        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<HttpResponseMessage> SaveOfficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "Document/v1/Document/uploadAgencyFile?saveAsFileName=" + saveAsFileName);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> SaveUnofficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "Document/v1/Document/uploadAgencyFile?saveAsFileName" + saveAsFileName);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }
}
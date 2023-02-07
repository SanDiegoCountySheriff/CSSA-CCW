using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text.Json;

namespace CCW.Application.Clients;

public class DocumentServiceClient : IDocumentServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string applicationTemplate;
    private readonly string unofficialPermitTemplate;
    private readonly string officialPermitTemplate;
    private readonly string downloadAgencyUri;
    private readonly string downloadApplicantUri;
    private readonly string uploadApplicantUri;

    public DocumentServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        applicationTemplate = configuration.GetSection("DocumentApi").GetSection("ApplicationTemplateName").Value;
        unofficialPermitTemplate = configuration.GetSection("DocumentApi").GetSection("UnofficalLicenseTemplateName").Value;
        officialPermitTemplate = configuration.GetSection("DocumentApi").GetSection("OfficialLicenseTemplateName").Value;
        downloadAgencyUri = configuration.GetSection("DocumentServiceClient").GetSection("DownloadAgencyBaseUrl").Value;
        downloadApplicantUri = configuration.GetSection("DocumentServiceClient").GetSection("DownloadApplicantBaseUrl").Value;
        uploadApplicantUri = configuration.GetSection("DocumentServiceClient").GetSection("UploadApplicantBaseUrl").Value;
    }

    public async Task<HttpResponseMessage> GetApplicantImageAsync(string fileName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, downloadApplicantUri + fileName);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> GetApplicationTemplateAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, downloadAgencyUri + applicationTemplate);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> GetOfficialLicenseTemplateAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, downloadAgencyUri + officialPermitTemplate);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> GetUnofficialLicenseTemplateAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, downloadAgencyUri + unofficialPermitTemplate);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> SaveApplicationPdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uploadApplicantUri + saveAsFileName);
        var form = new MultipartFormDataContent();

        var streamContent = new StreamContent(fileToUpload.OpenReadStream());

        var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());

      //  fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

        // "file" parameter name should be the same as the server side input parameter name
        form.Add(fileContent, "fileToUpload", saveAsFileName);
        request.Content = form;
        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<HttpResponseMessage> SaveOfficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
          var request = new HttpRequestMessage(HttpMethod.Post, uploadApplicantUri + saveAsFileName);

        var multiContent = new MultipartFormDataContent();

        var streamContent = new StreamContent(fileToUpload.OpenReadStream());
        streamContent.Headers.Add("Content-Type", "application/octet-stream");
        streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"file1\"; filename=\"{saveAsFileName}\"");

        var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");

        multiContent.Add(streamContent, "file", saveAsFileName);

        HttpResponseMessage response = await _httpClient.PostAsync(uploadApplicantUri + saveAsFileName, multiContent);
        response.EnsureSuccessStatusCode();

        var content2 = await response.Content.ReadAsStringAsync();

        return response;
    }

    public async Task<HttpResponseMessage> SaveUnofficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uploadApplicantUri + saveAsFileName);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }
}
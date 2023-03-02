using System.Net.Http.Headers;

namespace CCW.Application.Clients;

public class DocumentServiceClient : IDocumentServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string sheriffSignature;
    private readonly string applicationTemplate;
    private readonly string unofficialPermitTemplate;
    private readonly string officialPermitTemplate;
    private readonly string downloadAgencyUri;
    private readonly string downloadApplicantUri;
    private readonly string uploadApplicantUri;

    public DocumentServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        sheriffSignature = configuration.GetSection("DocumentApi").GetSection("SheriffSignature").Value;
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

    public async Task<HttpResponseMessage> GetSheriffSignatureAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, downloadAgencyUri + sheriffSignature);

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
        var multiContent = new MultipartFormDataContent();
        var streamContent = new StreamContent(fileToUpload.OpenReadStream());

        streamContent.Headers.Add("Content-Type", "application/pdf");
        streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"fileToUpload\"; filename=\"{saveAsFileName}\"");

        var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

        multiContent.Add(fileContent, "fileToUpload", saveAsFileName);

        HttpResponseMessage response = await _httpClient.PostAsync(uploadApplicantUri + saveAsFileName, multiContent);
        response.EnsureSuccessStatusCode();

        await response.Content.ReadAsStringAsync();

        return response;
    }

    public async Task<HttpResponseMessage> SaveOfficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var multiContent = new MultipartFormDataContent();
        var streamContent = new StreamContent(fileToUpload.OpenReadStream());

        streamContent.Headers.Add("Content-Type", "application/pdf");
        streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"fileToUpload\"; filename=\"{saveAsFileName}\"");

        var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

        multiContent.Add(fileContent, "fileToUpload", saveAsFileName);

        HttpResponseMessage response = await _httpClient.PostAsync(uploadApplicantUri + saveAsFileName, multiContent);
        response.EnsureSuccessStatusCode();
        
        await response.Content.ReadAsStringAsync();

        return response;
    }

    public async Task<HttpResponseMessage> SaveUnofficialLicensePdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var multiContent = new MultipartFormDataContent();
        var streamContent = new StreamContent(fileToUpload.OpenReadStream());

        streamContent.Headers.Add("Content-Type", "application/pdf");
        streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"fileToUpload\"; filename=\"{saveAsFileName}\"");

        var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

        multiContent.Add(fileContent, "fileToUpload", saveAsFileName);

        HttpResponseMessage response = await _httpClient.PostAsync(uploadApplicantUri + saveAsFileName, multiContent);
        response.EnsureSuccessStatusCode();

        await response.Content.ReadAsStringAsync();

        return response;
    }
}
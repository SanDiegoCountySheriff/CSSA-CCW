using System.Net;
using System.Net.Http.Headers;

namespace CCW.Application.Clients;

public class DocumentServiceClient : IDocumentServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string sheriffSignature;
    private readonly string sheriffLogo;
    private readonly string processorsSignature;
    private readonly string applicationTemplate;
    private readonly string unofficialPermitTemplate;
    private readonly string officialPermitTemplate;
    private readonly string liveScanTemplate;
    private readonly string downloadAgencyUri;
    private readonly string downloadApplicantUri;
    private readonly string downloadAdminUserFileUri;
    private readonly string uploadApplicantUri;
    private readonly string uploadAdminApplicationUri;

    public DocumentServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        var documentSettings = configuration.GetSection("DocumentApi");
        sheriffSignature = documentSettings.GetSection("SheriffSignature").Value;
        sheriffLogo = documentSettings.GetSection("SheriffLogo").Value;
        processorsSignature = documentSettings.GetSection("ProcessorSignature").Value;
        applicationTemplate = documentSettings.GetSection("ApplicationTemplateName").Value;
        unofficialPermitTemplate = documentSettings.GetSection("UnofficalLicenseTemplateName").Value;
        officialPermitTemplate = documentSettings.GetSection("OfficialLicenseTemplateName").Value;
        liveScanTemplate = documentSettings.GetSection("LiveScanTemplateName").Value;

        var documentClientSettings = configuration.GetSection("DocumentServiceClient");
        downloadAgencyUri = documentClientSettings.GetSection("DownloadAgencyBaseUrl").Value;
        downloadApplicantUri = documentClientSettings.GetSection("DownloadApplicantBaseUrl").Value;
        downloadAdminUserFileUri = documentClientSettings.GetSection("DownloadAdminUserFileBaseUrl").Value;
        uploadApplicantUri = documentClientSettings.GetSection("UploadApplicantBaseUrl").Value;
        uploadAdminApplicationUri = documentClientSettings.GetSection("UploadAdminApplicationBaseUrl").Value;
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

    public async Task<HttpResponseMessage> GetLiveScanTemplateAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, downloadAgencyUri + liveScanTemplate);

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

    public async Task<HttpResponseMessage> GetSheriffLogoAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, downloadAgencyUri + sheriffLogo);

        var result = await _httpClient.SendAsync(request, cancellationToken);
        result.EnsureSuccessStatusCode();

        return result;
    }

    public async Task<HttpResponseMessage> GetProcessorSignatureAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, downloadAdminUserFileUri + processorsSignature);

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

    public async Task<HttpResponseMessage> SaveAdminApplicationPdfAsync(IFormFile fileToUpload, string saveAsFileName, CancellationToken cancellationToken)
    {
        var multiContent = new MultipartFormDataContent();
        var streamContent = new StreamContent(fileToUpload.OpenReadStream());

        streamContent.Headers.Add("Content-Type", "application/pdf");
        streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"fileToUpload\"; filename=\"{saveAsFileName}\"");

        var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");

        multiContent.Add(fileContent, "fileToUpload", saveAsFileName);

        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync(uploadAdminApplicationUri + saveAsFileName, multiContent);
            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync();
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("An error occurred: " + ex.Message)
            };
        }

    }
}
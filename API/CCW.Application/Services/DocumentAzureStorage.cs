using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CCW.Application.Services.Contracts;
using System.Net;

namespace CCW.Application.Services;

public class DocumentAzureStorage : IDocumentAzureStorage
{
    private readonly string _storageConnection;
    private readonly string _agencyContainerName;
    private readonly string _publicContainerName;
    private readonly string _adminUserContainerName;
    private readonly string _adminApplicationContainerName;
    private readonly BlobClientOptions _blobClientOptions;
    private readonly string sheriffSignature;
    private readonly string sheriffLogo;
    private readonly string applicationTemplate;
    private readonly string unofficialPermitTemplate;
    private readonly string officialPermitTemplate;
    private readonly string liveScanTemplate;
    private readonly string revocationLetterTemplate;
    private readonly string adminSignatureFileName;

    public DocumentAzureStorage(IConfiguration configuration)
    {
        var documentSettings = configuration.GetSection("DocumentApi");
        sheriffSignature = documentSettings.GetSection("SheriffSignature").Value;
        sheriffLogo = documentSettings.GetSection("SheriffLogo").Value;
        applicationTemplate = documentSettings.GetSection("ApplicationTemplateName").Value;
        unofficialPermitTemplate = documentSettings.GetSection("UnofficalLicenseTemplateName").Value;
        officialPermitTemplate = documentSettings.GetSection("OfficialLicenseTemplateName").Value;
        liveScanTemplate = documentSettings.GetSection("LiveScanTemplateName").Value;
        revocationLetterTemplate = documentSettings.GetSection("RevocationTemplateName").Value;
        adminSignatureFileName = documentSettings.GetSection("AdminSignatureFileName").Value;

        var client = new SecretClient(new Uri(configuration.GetSection("KeyVault:VaultUri").Value),
            credential: new DefaultAzureCredential());
#if DEBUG
        _storageConnection = configuration.GetSection("Storage").GetSection("LocalConnectionString").Value;
#else
        _storageConnection = client.GetSecret("storage-ct-connection-primary").Value.Value;
#endif
        _agencyContainerName = configuration.GetSection("Storage").GetSection("AgencyContainerName").Value;
        _publicContainerName = configuration.GetSection("Storage").GetSection("PublicContainerName").Value;
        _adminUserContainerName = configuration.GetSection("Storage").GetSection("AdminUserContainerName").Value;
        _adminApplicationContainerName = configuration.GetSection("Storage").GetSection("AdminApplicationContainerName").Value;
        var handler = new HttpClientHandler()
        {
            Proxy = new WebProxy()
            {
                BypassProxyOnLocal = true
            }
        };
        _blobClientOptions = new BlobClientOptions()
        {
            Transport = new HttpClientTransport(handler)
        };
    }

    public async Task<Stream> GetAgreementPDF(string fileName, CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(fileName);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task<Stream> GetApplicantImageAsync(string fileName, CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(fileName);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task<Stream> GetApplicationTemplateAsync(CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(applicationTemplate);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task<Stream> GetLiveScanTemplateAsync(CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(liveScanTemplate);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task<Stream> GetOfficialLicenseTemplateAsync(CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(officialPermitTemplate);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task<Stream> GetProcessorSignatureAsync(string processorUserName, CancellationToken cancellationToken)
    {
        var adminUserFileName = $"{processorUserName}_{adminSignatureFileName}";

        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminUserContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminUserContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(adminUserFileName);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task<Stream> GetRevocationLetterTemplateAsync(CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(revocationLetterTemplate);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task<Stream> GetSheriffLogoAsync(CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(sheriffLogo);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task<Stream> GetSheriffSignatureAsync(CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(sheriffSignature);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task<Stream> GetUnofficialLicenseTemplateAsync(CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(unofficialPermitTemplate);

        await file.DownloadToAsync(ms, cancellationToken);
        Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

        return blobStream;
    }

    public async Task SaveAdminApplicationPdfAsync(FormFile fileToSave, string fileName, CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminApplicationContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminApplicationContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        BlobClient blob = container.GetBlobClient(fileName);

        using (Stream file = fileToSave.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = fileToSave.ContentType }, cancellationToken: cancellationToken);
        }
    }
}

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
    private readonly string _sheriffSignature;
    private readonly string _sheriffLogo;
    private readonly string _applicationTemplate;
    private readonly string _unofficialPermitTemplate;
    private readonly string _modificationTemplate;
    private readonly string _officialPermitTemplate;
    private readonly string _liveScanTemplate;
    private readonly string _revocationLetterTemplate;
    private readonly string _adminSignatureFileName;

    public DocumentAzureStorage(IConfiguration configuration)
    {
        var documentSettings = configuration.GetSection("DocumentApi");
        _sheriffSignature = documentSettings.GetSection("SheriffSignature").Value;
        _sheriffLogo = documentSettings.GetSection("SheriffLogo").Value;
        _applicationTemplate = documentSettings.GetSection("ApplicationTemplateName").Value;
        _unofficialPermitTemplate = documentSettings.GetSection("UnofficalLicenseTemplateName").Value;
        _officialPermitTemplate = documentSettings.GetSection("OfficialLicenseTemplateName").Value;
        _liveScanTemplate = documentSettings.GetSection("LiveScanTemplateName").Value;
        _revocationLetterTemplate = documentSettings.GetSection("RevocationTemplateName").Value;
        _adminSignatureFileName = documentSettings.GetSection("AdminSignatureFileName").Value;
        _modificationTemplate = documentSettings.GetSection("AmendmentTemplateName").Value;

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
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        return await container.GetBlobClient(fileName).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<byte[]> GetApplicantImageAsync(string fileName, CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _publicContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        var file = container.GetBlobClient(fileName);

        using (var ms = new MemoryStream())
        {
            file.DownloadTo(ms, cancellationToken);
            return ms.ToArray();
        }
    }

    public async Task<Stream> GetApplicationTemplateAsync(CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        return await container.GetBlobClient(_applicationTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetLiveScanTemplateAsync(CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        return await container.GetBlobClient(_liveScanTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetOfficialLicenseTemplateAsync(CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        return await container.GetBlobClient(_officialPermitTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<byte[]> GetProcessorSignatureAsync(string processorUserName, CancellationToken cancellationToken)
    {
        var adminUserFileName = $"{processorUserName}_{_adminSignatureFileName}";
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminUserContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _adminUserContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        BlobClient file;

        file = container.GetBlobClient(adminUserFileName);

        using (var ms = new MemoryStream())
        {
            file.DownloadTo(ms, cancellationToken);
            return ms.ToArray();
        }
    }

    public async Task<Stream> GetRevocationLetterTemplateAsync(CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        return await container.GetBlobClient(_revocationLetterTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetSheriffLogoAsync(CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        return await container.GetBlobClient(_sheriffLogo).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetSheriffSignatureAsync(CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        return await container.GetBlobClient(_sheriffSignature).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetUnofficialLicenseTemplateAsync(CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        return await container.GetBlobClient(_unofficialPermitTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }
    public async Task<Stream> GetModificationTemplateAsync(CancellationToken cancellationToken)
    {
#if DEBUG
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName, _blobClientOptions);
#else
        BlobContainerClient container = new BlobContainerClient(_storageConnection, _agencyContainerName);
#endif
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        return await container.GetBlobClient(_modificationTemplate).OpenReadAsync(cancellationToken: cancellationToken);
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
            blob.Upload(file, new BlobHttpHeaders { ContentType = "application/pdf" }, cancellationToken: cancellationToken);
        }
    }
}

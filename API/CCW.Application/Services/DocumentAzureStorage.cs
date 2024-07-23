using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CCW.Application.Services.Contracts;
using CCW.Common.Services.Contracts;

namespace CCW.Application.Services;

public class DocumentAzureStorage : IDocumentAzureStorage
{
    private readonly string _sheriffSignature;
    private readonly string _sheriffLogo;
    private readonly string _applicationTemplate;
    private readonly string _legacyApplicationTemplate;
    private readonly string _unofficialPermitTemplate;
    private readonly string _modificationTemplate;
    private readonly string _officialPermitTemplate;
    private readonly string _liveScanTemplate;
    private readonly string _revocationLetterTemplate;
    private readonly string _adminSignatureFileName;
    private readonly BlobContainerClient _agencyContainer;
    private readonly BlobContainerClient _publicContainer;
    private readonly BlobContainerClient _adminUserContainer;
    private readonly BlobContainerClient _adminApplicationContainer;
    private readonly IStorageContainerResolver _storageContainerResolver;
    private readonly IHttpContextAccessor _contextAccessor;

    public DocumentAzureStorage(
        IStorageContainerResolver storageContainerResolver,
        IHttpContextAccessor contextAccessor,
        IConfiguration configuration
    )
    {
        _storageContainerResolver = storageContainerResolver;
        _contextAccessor = contextAccessor;

        var tenantId = _contextAccessor.HttpContext.Items["TenantId"] != null ? _contextAccessor.HttpContext.Items["TenantId"].ToString() : "";
        var agencyContainerName = configuration.GetSection("Storage").GetSection("AgencyContainerName").Value;
        var publicContainerName = configuration.GetSection("Storage").GetSection("PublicContainerName").Value;
        var adminUserContainerName = configuration.GetSection("Storage").GetSection("AdminUserContainerName").Value;
        var adminApplicationContainerName = configuration.GetSection("Storage").GetSection("AdminApplicationContainerName").Value;

        if (!string.IsNullOrEmpty(tenantId))
        {
            _agencyContainer = _storageContainerResolver.GetBlobContainer(tenantId, agencyContainerName);
            _publicContainer = _storageContainerResolver.GetBlobContainer(tenantId, publicContainerName);
            _adminUserContainer = _storageContainerResolver.GetBlobContainer(tenantId, adminUserContainerName);
            _adminApplicationContainer = _storageContainerResolver.GetBlobContainer(tenantId, adminApplicationContainerName);
        }

        var documentSettings = configuration.GetSection("DocumentApi");
        _sheriffSignature = documentSettings.GetSection("SheriffSignature").Value;
        _sheriffLogo = documentSettings.GetSection("SheriffLogo").Value;
        _applicationTemplate = documentSettings.GetSection("ApplicationTemplateName").Value;
        _legacyApplicationTemplate = documentSettings.GetSection("LegacyApplicationTemplateName").Value;
        _unofficialPermitTemplate = documentSettings.GetSection("UnofficalLicenseTemplateName").Value;
        _officialPermitTemplate = documentSettings.GetSection("OfficialLicenseTemplateName").Value;
        _liveScanTemplate = documentSettings.GetSection("LiveScanTemplateName").Value;
        _revocationLetterTemplate = documentSettings.GetSection("RevocationTemplateName").Value;
        _adminSignatureFileName = documentSettings.GetSection("AdminSignatureFileName").Value;
        _modificationTemplate = documentSettings.GetSection("AmendmentTemplateName").Value;
    }

    public async Task<Stream> GetAgreementPDF(string fileName, CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(fileName).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<byte[]> GetApplicantImageAsync(string fileName, CancellationToken cancellationToken)
    {
        var file = _publicContainer.GetBlobClient(fileName);

        using (var ms = new MemoryStream())
        {
            file.DownloadTo(ms, cancellationToken);
            return ms.ToArray();
        }
    }

    public async Task<Stream> GetApplicationTemplateAsync(CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(_applicationTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetLegacyApplicationTemplateAsync(CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(_legacyApplicationTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetLiveScanTemplateAsync(CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(_liveScanTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetOfficialLicenseTemplateAsync(CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(_officialPermitTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<byte[]> GetProcessorSignatureAsync(string processorUserName, CancellationToken cancellationToken)
    {
        var adminUserFileName = $"{processorUserName}_{_adminSignatureFileName}";
        BlobClient file;

        file = _adminUserContainer.GetBlobClient(adminUserFileName);

        using (var ms = new MemoryStream())
        {
            file.DownloadTo(ms, cancellationToken);
            return ms.ToArray();
        }
    }

    public async Task<Stream> GetRevocationLetterTemplateAsync(CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(_revocationLetterTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetSheriffLogoAsync(CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(_sheriffLogo).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetSheriffSignatureAsync(CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(_sheriffSignature).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> GetUnofficialLicenseTemplateAsync(CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(_unofficialPermitTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }
    public async Task<Stream> GetModificationTemplateAsync(CancellationToken cancellationToken)
    {
        return await _agencyContainer.GetBlobClient(_modificationTemplate).OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task SaveAdminApplicationPdfAsync(FormFile fileToSave, string fileName, CancellationToken cancellationToken)
    {
        BlobClient blob = _adminApplicationContainer.GetBlobClient(fileName);

        using (Stream file = fileToSave.OpenReadStream())
        {
            blob.Upload(file, new BlobHttpHeaders { ContentType = "application/pdf" }, cancellationToken: cancellationToken);
        }
    }
}

namespace CCW.Application.Services.Contracts;

public interface IDocumentAzureStorage
{
    Task<Stream> GetApplicationTemplateAsync(CancellationToken cancellationToken);
    Task<Stream> GetRevocationLetterTemplateAsync(CancellationToken cancellationToken);
    Task<Stream> GetOfficialLicenseTemplateAsync(CancellationToken cancellationToken);
    Task<Stream> GetUnofficialLicenseTemplateAsync(CancellationToken cancellationToken);
    Task<Stream> GetModificationTemplateAsync(CancellationToken cancellationToken);
    Task<Stream> GetLiveScanTemplateAsync(CancellationToken cancellationToken);
    Task<byte[]> GetApplicantImageAsync(string fileName, CancellationToken cancellationToken);
    Task<byte[]> GetProcessorSignatureAsync(string processorUserName, CancellationToken cancellationToken);
    Task<Stream> GetSheriffSignatureAsync(CancellationToken cancellationToken);
    Task<Stream> GetSheriffLogoAsync(CancellationToken cancellationToken);
    Task<Stream> GetAgreementPDF(string fileName, CancellationToken cancellationToken);
    Task SaveAdminApplicationPdfAsync(FormFile fileToSave, string fileName, CancellationToken cancellationToken);
}

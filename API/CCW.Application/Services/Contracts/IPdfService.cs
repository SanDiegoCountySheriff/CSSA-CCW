using CCW.Common.Models;

namespace CCW.Application.Services.Contracts;

public interface IPdfService
{
    public Task<MemoryStream> GetApplicationMemoryStream(PermitApplication userApplication, string licensingUserName, string fileName);
    public Task<MemoryStream> GetLegacyApplicationMemoryStream(PermitApplication userApplication, string licensingUserName, string fileName);
    public Task<MemoryStream> GetModificationMemoryStream(PermitApplication userApplication, string licensingUserName, string fileName);
    public Task<MemoryStream> GetRevocationLetterMemoryStream(PermitApplication userApplication, string user, string licensingUserName, string reason, string date, string fileName, string licensingEmail);
    public Task<MemoryStream> GetOfficialLicenseMemoryStream(PermitApplication userApplication, string licensingUser, string fileName);
    public Task<MemoryStream> GetUnofficialLicenseMemoryStream(PermitApplication userApplication, string fileName);
    public Task<MemoryStream> GetLivescanMemoryStream(PermitApplication userApplication, string fileName);
}

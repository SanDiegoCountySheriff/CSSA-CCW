using CCW.Application.Entities;

namespace CCW.Application.Services.Contracts;

public interface IPdfService
{
    public Task<MemoryStream> GetApplicationMemoryStream(PermitApplication userApplication, string licensingUserName, string fileName);
    public Task<MemoryStream> GetRevocationLetterMemoryStream(PermitApplication userApplication, string user, string licensingUserName, string reason, string date, string fileName);
    public Task<MemoryStream> GetOfficialLicenseMemoryStream(PermitApplication userApplication, string licensingUser, string fileName);
}

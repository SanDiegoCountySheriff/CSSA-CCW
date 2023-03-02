using CCW.Application.Models;

namespace CCW.Application.Clients;

public interface IAdminServiceClient
{
    Task<AgencyProfileSettingsModel> GetAgencyProfileSettingsAsync(CancellationToken cancellationToken);
}
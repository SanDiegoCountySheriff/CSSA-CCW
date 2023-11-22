using CCW.Common.Models;

namespace CCW.Application.Clients;

public interface IAdminServiceClient
{
    Task<AgencyProfileSettings> GetAgencyProfileSettingsAsync(CancellationToken cancellationToken);
}
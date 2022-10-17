using CCW.Admin.Models;

namespace CCW.Admin.Services;

public interface ICosmosDbService
{
    Task<AgencyProfileSettings> GetSettingsAsync(CancellationToken cancellationToken);
    Task<AgencyProfileSettings> AddSettingsAsync(AgencyProfileSettings agencyProfile, CancellationToken cancellationToken);
    Task<AgencyProfileSettings> UpdateSettingsAsync(AgencyProfileSettings agencyProfile, CancellationToken cancellationToken);
}

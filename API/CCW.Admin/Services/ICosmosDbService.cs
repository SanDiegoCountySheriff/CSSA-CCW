using CCW.Common.Models;

namespace CCW.Admin.Services;

public interface ICosmosDbService
{
    Task<AgencyProfileSettings> GetSettingsAsync(string tenantId, CancellationToken cancellationToken);
    Task<AgencyProfileSettings> UpdateSettingsAsync(AgencyProfileSettings agencyProfile, CancellationToken cancellationToken);
}
using CCW.Admin.Models;

namespace CCW.Admin.Services;

public interface ICosmosDbService
{
    Task<AgencyProfileSettings> GetSettingsAsync(string agencyId, CancellationToken cancellationToken);
    Task<AgencyProfileSettings> AddAsync(AgencyProfileSettings agencyProfile, CancellationToken cancellationToken);
}

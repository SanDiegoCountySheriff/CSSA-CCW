using CCW.Admin.Entities;


namespace CCW.Admin.Services;

public interface ICosmosDbService
{
    Task<AgencyProfileSettings?> GetSettingsAsync(CancellationToken cancellationToken);
    Task<AgencyProfileSettings> UpdateSettingsAsync(AgencyProfileSettings agencyProfile, CancellationToken cancellationToken);
}
using CCW.Common.Models;

namespace CCW.Application.Services.Contracts;

public interface IAdminCosmosDbService
{
    Task<AgencyProfileSettings> GetAgencyProfileSettingsAsync(CancellationToken cancellationToken);
}

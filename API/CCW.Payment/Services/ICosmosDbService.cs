using CCW.Application.Entities;

namespace CCW.Payment.Services;

public interface ICosmosDbService
{
    Task<PermitApplication> GetApplication(string applicationId, string userId, CancellationToken cancellationToken);
    Task<PermitApplication> GetUserApplication(string applicationId, CancellationToken cancellationToken);
    Task UpdateApplication(PermitApplication application);
}

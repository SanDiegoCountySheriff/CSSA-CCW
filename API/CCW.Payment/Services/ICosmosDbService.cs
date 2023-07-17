using CCW.Application.Entities;
using CCW.Common.Models;

namespace CCW.Payment.Services;

public interface ICosmosDbService
{
    Task<PermitApplication> GetApplication(string applicationId, string userId, CancellationToken cancellationToken);
    Task UpdateApplication(PermitApplication application);
}

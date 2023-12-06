using CCW.Common.Models;

namespace CCW.Schedule.Services.Contracts;

public interface IApplicationCosmosDbService
{
    Task<PermitApplication> GetUserApplicationAsync(string applicationId, CancellationToken cancellationToken);
    Task UpdateUserApplicationAsync(PermitApplication application, CancellationToken cancellationToken);
}

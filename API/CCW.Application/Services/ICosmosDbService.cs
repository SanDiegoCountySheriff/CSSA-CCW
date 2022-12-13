using CCW.Application.Entities;


namespace CCW.Application.Services;

public interface ICosmosDbService
{
    Task<PermitApplication> AddAsync(PermitApplication application, CancellationToken cancellationToken);
    Task<PermitApplication?> GetLastApplicationAsync(string userEmail, bool isOrderId, bool isComplete, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetAllUserApplicationsAsync(string userEmail, CancellationToken cancellationToken);
    Task<IEnumerable<History>> GetApplicationHistoryAsync(string applicationIdOrOrderId, CancellationToken cancellationToken, bool isOrderId = false);
    Task<IEnumerable<SummarizedPermitApplication>> GetAllApplicationsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> SearchApplicationsAsync(string searchValue, CancellationToken cancellationToken);
    Task UpdateAsync(PermitApplication application, CancellationToken cancellationToken);
}

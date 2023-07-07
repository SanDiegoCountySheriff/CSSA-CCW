using CCW.Application.Entities;


namespace CCW.Application.Services;

public interface ICosmosDbService
{
    Task<PermitApplication> AddAsync(PermitApplication application, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetAllOpenApplicationsForUserAsync(string userId,
        CancellationToken cancellationToken);
    Task<string> GetSSNAsync(string userId, CancellationToken cancellationToken);
    Task<PermitApplication?> GetLastApplicationAsync(string userId, string applicationId, CancellationToken cancellationToken);
    Task<PermitApplication?> GetUserLastApplicationAsync(string userEmailOrOrderId, bool isOrderId, bool isComplete, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetAllApplicationsAsync(string userId, string userEmail, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetAllUserApplicationsAsync(string userEmail, CancellationToken cancellationToken);
    Task<PermitApplication?> GetUserApplicationAsync(string applicationId, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetMultipleApplicationsAsync(string[] applicationIds, CancellationToken cancellationToken);
    Task<IEnumerable<History>> GetApplicationHistoryAsync(string applicationIdOrOrderId, CancellationToken cancellationToken, bool isOrderId = false);
    Task<IEnumerable<SummarizedPermitApplication>> GetAllInProgressApplicationsSummarizedAsync(CancellationToken cancellationToken);
    Task<IEnumerable<SummarizedPermitApplication>> SearchApplicationsAsync(string searchValue, CancellationToken cancellationToken);
    Task UpdateApplicationAsync(PermitApplication application, PermitApplication existingApplication, CancellationToken cancellationToken);
    Task UpdateUserApplicationAsync(PermitApplication application, CancellationToken cancellationToken);
    Task DeleteApplicationAsync(string userId, string applicationId, CancellationToken cancellationToken);
    Task DeleteUserApplicationAsync(string userId, string applicationId, CancellationToken cancellationToken);
}

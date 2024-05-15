using CCW.Common.Models;
using CCW.Common.ResponseModels;
using static CCW.Application.Controllers.PermitApplicationController;

namespace CCW.Application.Services.Contracts;

public interface IApplicationCosmosDbService
{
    Task<PermitApplication> AddAsync(PermitApplication application, CancellationToken cancellationToken);
    Task<PermitApplication> AddHistoricalApplicationAsync(PermitApplication application, CancellationToken cancellationToken);
    Task<PermitApplication> AddHistoricalApplicationPublicAsync(PermitApplication application, PermitApplication existingApplication, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetAllOpenApplicationsForUserAsync(string userId,
        CancellationToken cancellationToken);
    Task<string> GetSSNAsync(string userId, CancellationToken cancellationToken);
    Task<PermitApplication> GetLastApplicationAsync(string userId, string applicationId, CancellationToken cancellationToken);
    Task<PermitApplication> GetUserLastApplicationAsync(string userEmailOrOrderId, bool isOrderId, bool isComplete, bool isLegacy, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetAllApplicationsAsync(string userId, string userEmail, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetAllUserApplicationsAsync(string userEmail, CancellationToken cancellationToken);
    Task<PermitApplication> GetUserApplicationAsync(string applicationId, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetMultipleApplicationsAsync(string[] applicationIds, CancellationToken cancellationToken);
    Task<IEnumerable<History>> GetApplicationHistoryAsync(string applicationIdOrOrderId, CancellationToken cancellationToken, bool isOrderId = false);
    Task<(IEnumerable<SummarizedPermitApplication>, int)> GetAllInProgressApplicationsSummarizedAsync(PermitsOptions options, CancellationToken cancellationToken);
    Task<IEnumerable<SummarizedPermitApplication>> SearchApplicationsAsync(string searchValue, CancellationToken cancellationToken);
    Task UpdateApplicationAsync(PermitApplication application, PermitApplication existingApplication, CancellationToken cancellationToken);
    Task UpdateUserApplicationAsync(PermitApplication application, CancellationToken cancellationToken);
    Task DeleteUserApplicationAsync(string userId, string applicationId, CancellationToken cancellationToken);
    Task<int> GetApplicationCountAsync(PermitsOptions options, CancellationToken cancellationToken);
    Task<ApplicationSummaryCountResponseModel> GetApplicationSummaryCount(CancellationToken cancellationToken);
    Task<List<AssignedApplicationSummary>> GetAssignedApplicationsSummary(string userName, CancellationToken cancellationToken);
    Task<(IEnumerable<SummarizedLegacyApplication>, int)> GetAllLegacyApplicationsAsync(PermitsOptions options, CancellationToken cancellationToken);
    Task<PermitApplication> GetLegacyApplication(string applicationId, CancellationToken cancellationToken);
    Task UpdateLegacyApplication(PermitApplication application, bool createApplication, CancellationToken cancellationToken);
}

using CCW.Application.Entities;
using CCW.Application.Models;
using System.Threading;

namespace CCW.Application.Services;

public interface ICosmosDbService
{
    Task<IEnumerable<SummarizedPermitApplication>> GetAllApplicationsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> GetMultipleAsync(CancellationToken cancellationToken);
    Task<PermitApplication> GetAsync(string userEmail, bool isOrderId, bool isComplete, CancellationToken cancellationToken);
    Task<IEnumerable<PermitApplication>> ListAsync(int startIndex, int count, CancellationToken cancellationToken);
    Task<PermitApplication> AddAsync(PermitApplication application, CancellationToken cancellationToken);
    Task UpdateAsync(PermitApplication application, CancellationToken cancellationToken);
    Task DeleteAsync(string applicationId, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<History>> GetApplicationHistoryAsync(string applicationIdOrOrderId, CancellationToken cancellationToken, bool isOrderId = false);
}

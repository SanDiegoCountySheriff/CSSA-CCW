using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Services;

public interface ICosmosDbService
{
    Task<IEnumerable<SummarizedPermitApplication>> GetAllApplicationsAsync();
    Task<IEnumerable<PermitApplication>> GetMultipleAsync();
    Task<PermitApplication> GetAsync(string userEmail, bool isOrderId, bool isComplete);
    Task<IEnumerable<PermitApplication>> ListAsync(int startIndex, int count);
    Task<PermitApplication> AddAsync(PermitApplication application);
    Task UpdateAsync(PermitApplication application);
    Task DeleteAsync(string applicationId, string userId);
    Task<IEnumerable<History>> GetApplicationHistoryAsync(string applicationIdOrOrderId, bool isOrderId = false);
}

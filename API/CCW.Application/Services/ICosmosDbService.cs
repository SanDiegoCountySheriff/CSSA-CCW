using CCW.Application.Models;

namespace CCW.Application.Services;

    public interface ICosmosDbService
    {
    Task<IEnumerable<PermitApplication>> GetMultipleAsync(string query);
    Task<PermitApplication> GetAsync(string applicationId);
    Task<PermitApplication> AddAsync(PermitApplication application);
    Task UpdateAsync(PermitApplication application);
    Task DeleteAsync(string applicationId, string userId);
}

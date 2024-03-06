using CCW.Common.Models;


namespace CCW.UserProfile.Services;

public interface ICosmosDbService
{
    Task<AdminUser> AddAdminUserAsync(AdminUser adminUser, CancellationToken cancellationToken);
    Task<AdminUser> GetAdminUserAsync(string adminUserId, CancellationToken cancellationToken);
    Task<IEnumerable<AdminUser>> GetAllAdminUsers(CancellationToken cancellationToken);
}

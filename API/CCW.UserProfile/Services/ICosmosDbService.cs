using CCW.UserProfile.Entities;


namespace CCW.UserProfile.Services;

public interface ICosmosDbService
{
    Task<User?> GetAsync(string userId, CancellationToken cancellationToken);
    Task<User?> AddAsync(User user, CancellationToken cancellationToken);
    Task<AdminUser?> AddAdminUserAsync(AdminUser adminUser, CancellationToken cancellationToken);
    Task<AdminUser?> GetAdminUserAsync(string adminUserId, CancellationToken cancellationToken);
    Task<IEnumerable<AdminUser>> GetAllAdminUsers(CancellationToken cancellationToken);
}

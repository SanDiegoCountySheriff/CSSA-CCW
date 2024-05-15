using CCW.Common.Models;


namespace CCW.UserProfile.Services;

public interface ICosmosDbService
{
    Task<AdminUser> AddAdminUserAsync(AdminUser adminUser, CancellationToken cancellationToken);
    Task<AdminUser> GetAdminUserAsync(string adminUserId, CancellationToken cancellationToken);
    Task<IEnumerable<AdminUser>> GetAllAdminUsers(CancellationToken cancellationToken);
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
    Task<User> GetUserAsync(string userId, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllUsers(CancellationToken cancellationToken);
    Task<List<User>> GetUnmatchedUserProfiles(CancellationToken cancellationToken);
}

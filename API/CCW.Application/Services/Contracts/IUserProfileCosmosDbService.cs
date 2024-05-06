using CCW.Common.Models;

namespace CCW.Application.Services.Contracts;

public interface IUserProfileCosmosDbService
{
    Task<AdminUser> GetAdminUserProfileAsync(string licensingUserName, CancellationToken cancellationToken);
    Task UpdateUser(User user, CancellationToken cancellationToken);
    Task<User> GetUser(string id, CancellationToken cancellationToken);
}

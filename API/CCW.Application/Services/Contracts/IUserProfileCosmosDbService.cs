using CCW.Common.Models;

namespace CCW.Application.Services.Contracts;

public interface IUserProfileCosmosDbService
{
    Task<AdminUser> GetAdminUserProfileAsync(string licensingUserName, CancellationToken cancellationToken);
}

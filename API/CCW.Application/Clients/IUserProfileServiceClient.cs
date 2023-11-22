using CCW.Common.Models;

namespace CCW.Application.Clients
{
    public interface IUserProfileServiceClient
    {
        Task<AdminUser> GetAdminUserProfileAsync(CancellationToken cancellationToken);
    }
}

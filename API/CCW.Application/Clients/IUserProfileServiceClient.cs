using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Clients
{
    public interface IUserProfileServiceClient
    {
        Task<AdminUser> GetAdminUserProfileAsync(CancellationToken cancellationToken);
    }
}

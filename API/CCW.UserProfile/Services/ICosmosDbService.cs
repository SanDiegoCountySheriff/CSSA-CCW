using CCW.UserProfile.Models;

namespace CCW.UserProfile.Services;

public interface ICosmosDbService
{
    Task<IEnumerable<User>> GetMultipleAsync(string query);
    Task<User?> GetAsync(string email, CancellationToken cancellationToken);
    Task<User> AddAsync(User user);
    Task UpdateAsync(string id, User user);
    Task DeleteAsync(string id);
}

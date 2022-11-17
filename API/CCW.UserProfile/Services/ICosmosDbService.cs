using CCW.UserProfile.Entities;
using System.Threading;


namespace CCW.UserProfile.Services;

public interface ICosmosDbService
{
  //  Task<IEnumerable<User>> GetMultipleAsync(string query);
    Task<User?> GetAsync(string email, CancellationToken cancellationToken);
    // Task<User?> GetUserAsync(string id, CancellationToken cancellationToken);
    Task<User> AddAsync(User user, CancellationToken cancellationToken);
  //  Task UpdateAsync(string id, User user);
  //  Task DeleteAsync(string id);
}

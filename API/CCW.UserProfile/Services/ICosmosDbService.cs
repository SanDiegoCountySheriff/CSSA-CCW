using CCW.UserProfile.Entities;


namespace CCW.UserProfile.Services;

public interface ICosmosDbService
{
    Task<User?> GetAsync(string userId, CancellationToken cancellationToken);
    Task<User?> AddAsync(User user, CancellationToken cancellationToken);
}

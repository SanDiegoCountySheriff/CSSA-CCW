using CCW.UserProfile.Entities;
using CCW.UserProfile.Models;

namespace CCW.UserProfile.Mappers;

public class UserProfileRequestModelToEntityMapper : IMapper<UserProfileRequestModel, User>
{
    public User Map(UserProfileRequestModel source)
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = source.EmailAddress,
        };
    }
}
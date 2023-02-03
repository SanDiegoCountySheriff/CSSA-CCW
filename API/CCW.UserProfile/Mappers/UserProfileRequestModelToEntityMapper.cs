using CCW.UserProfile.Entities;
using CCW.UserProfile.Models;

namespace CCW.UserProfile.Mappers;

public class UserProfileRequestModelToEntityMapper : IMapper<string, UserProfileRequestModel, User>
{
    public User Map(string userId, UserProfileRequestModel source)
    {
        return new User
        {
            Id = userId,
            UserEmail = source.EmailAddress,
        };
    }
}
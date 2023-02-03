using CCW.UserProfile.Entities;
using CCW.UserProfile.Models;

namespace CCW.UserProfile.Mappers;

public class EntityToUserProfileResponseModelMapper : IMapper<User, UserProfileResponseModel>
{
    public UserProfileResponseModel Map(User source)
    {
        return new UserProfileResponseModel
        {
            UserEmail = source.UserEmail,
            Id = source.Id,
            PreviousEmails = source.PreviousEmails,
        };
    }
}

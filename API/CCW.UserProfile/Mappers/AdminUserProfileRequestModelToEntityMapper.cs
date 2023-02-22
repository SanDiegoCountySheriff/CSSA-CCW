using CCW.UserProfile.Entities;
using CCW.UserProfile.Models;

namespace CCW.UserProfile.Mappers
{
    public class AdminUserProfileRequestModelToEntityMapper : IMapper<string, AdminUserProfileRequestModel, AdminUser>
    {
        public AdminUser Map(string userId, AdminUserProfileRequestModel source2)
        {
            return new AdminUser()
            {
                Id = userId,
                BadgeNumber = source2.BadgeNumber,
                UploadedDocuments = source2.UploadedDocuments
            };
        }
    }
}

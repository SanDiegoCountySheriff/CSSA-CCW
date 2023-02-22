using CCW.UserProfile.Entities;
using CCW.UserProfile.Models;

namespace CCW.UserProfile.Mappers
{
    public class EntityToAdminUserProfileResponseModelMapper : IMapper<AdminUser, AdminUserProfileResponseModel>
    {
        public AdminUserProfileResponseModel Map(AdminUser sourceType)
        {
            return new AdminUserProfileResponseModel()
            {
                Id = sourceType.Id,
                BadgeNumber = sourceType.BadgeNumber,
                UploadedDocuments = sourceType.UploadedDocuments,
            };
        }
    }
}

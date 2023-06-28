using CCW.UserProfile.Entities;
using CCW.UserProfile.Models;

namespace CCW.UserProfile.Mappers
{
    public class AllAdminUsersToAdminUserProfileResponseModelMapper : IMapper<IEnumerable<AdminUser>, IEnumerable<AdminUserProfileResponseModel>>
    {
        public IEnumerable<AdminUserProfileResponseModel> Map(IEnumerable<AdminUser> sourceType)
        {
            List<AdminUserProfileResponseModel> response = new();

            foreach (var adminUser in sourceType)
            {
                var adminUserResponse = new AdminUserProfileResponseModel()
                {
                    BadgeNumber = adminUser.BadgeNumber,
                    Name = adminUser.Name,
                    Id = adminUser.Id,
                    UploadedDocuments = Array.Empty<UploadedDocument>(),
                };

                response.Add(adminUserResponse);
            }

            return response;
        }
    }
}
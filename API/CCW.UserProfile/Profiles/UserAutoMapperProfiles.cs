using AutoMapper;
using CCW.Common.Models;
using CCW.Common.RequestModels;
using CCW.Common.ResponseModels;

namespace CCW.UserProfile.Profiles;

public class UserAutoMapperProfiles : Profile
{
    public UserAutoMapperProfiles()
    {
        CreateMap<AdminUserProfileRequestModel, AdminUser>();
        CreateMap<AdminUser, AdminUserProfileResponseModel>();
    }
}

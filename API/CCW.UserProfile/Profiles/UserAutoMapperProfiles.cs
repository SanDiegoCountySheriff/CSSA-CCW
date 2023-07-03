using AutoMapper;
using CCW.UserProfile.Entities;
using CCW.UserProfile.Models;

namespace CCW.UserProfile.Profiles;

public class UserAutoMapperProfiles : Profile
{
    public UserAutoMapperProfiles()
    {
        CreateMap<AdminUserProfileRequestModel, AdminUser>();
        CreateMap<AdminUser, AdminUserProfileResponseModel>();
        CreateMap<UserProfileRequestModel, User>();
        CreateMap<User, UserProfileResponseModel>();
    }
}

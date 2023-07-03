using AutoMapper;
using CCW.Admin.Entities;
using CCW.Admin.Models;

namespace CCW.Admin.Profiles;

public class AdminAutoMapperProfiles : Profile
{
    public AdminAutoMapperProfiles()
    {
        CreateMap<AgencyProfileSettingsRequestModel, AgencyProfileSettings>();
        CreateMap<AgencyProfileSettings, AgencyProfileSettingsResponseModel>();
    }
}

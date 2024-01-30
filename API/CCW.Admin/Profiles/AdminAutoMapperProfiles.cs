using AutoMapper;
using CCW.Common.Models;
using CCW.Common.RequestModels;
using CCW.Common.ResponseModels;

namespace CCW.Admin.Profiles;

public class AdminAutoMapperProfiles : Profile
{
    public AdminAutoMapperProfiles()
    {
        CreateMap<AgencyProfileSettingsRequestModel, AgencyProfileSettings>();
        CreateMap<AgencyProfileSettings, AgencyProfileSettingsResponseModel>();
    }
}

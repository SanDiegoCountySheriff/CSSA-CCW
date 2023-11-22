using AutoMapper;
using CCW.Application.Models;
using CCW.Common.Models;

namespace CCW.Application.Profiles;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<PermitApplication, UserPermitApplicationResponseModel>();
        CreateMap<Common.Models.Application, UserApplication>();
        CreateMap<UserApplication, Common.Models.Application>();
        CreateMap<PermitApplicationRequestModel, PermitApplication>();
        CreateMap<PermitApplication, PermitApplicationResponseModel>();
        CreateMap<UserPermitApplicationRequestModel, PermitApplication>();
        CreateMap<PermitApplicationRequestModel, PermitApplication>();
        CreateMap<PermitApplication, UserPermitApplicationResponseModel>();
        CreateMap<PermitApplication, PermitApplicationResponseModel>();
        CreateMap<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel>();
        CreateMap<History, HistoryResponseModel>();
    }
}

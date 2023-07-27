using AutoMapper;
using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Profiles;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<PermitApplication, UserPermitApplicationResponseModel>();
        CreateMap<Entities.Application, UserApplication>();
        CreateMap<UserApplication, Entities.Application>();
        CreateMap<PermitApplicationRequestModel, PermitApplication>();
        CreateMap<PermitApplication, PermitApplicationResponseModel>();
        CreateMap<UserPermitApplicationRequestModel, PermitApplication>();
        CreateMap<PermitApplicationRequestModel, PermitApplication>();
        CreateMap<PermitApplication, UserPermitApplicationResponseModel>();
        CreateMap<PermitApplication, PermitApplicationResponseModel>();
        CreateMap<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel>();
        CreateMap<History,  HistoryResponseModel>();
    }
}

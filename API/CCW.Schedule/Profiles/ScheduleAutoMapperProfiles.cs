using AutoMapper;
using CCW.Common.Models;
using CCW.Common.RequestModels;
using CCW.Common.ResponseModels;

namespace CCW.Schedule.Profiles;

public class ScheduleAutoMapperProfiles : Profile
{
    public ScheduleAutoMapperProfiles()
    {
        CreateMap<AppointmentManagementRequestModel, AppointmentManagement>();
        CreateMap<AppointmentWindowCreateRequestModel, AppointmentWindow>();
        CreateMap<AppointmentWindow, AppointmentWindowResponseModel>();
        CreateMap<AppointmentWindowUpdateRequestModel, AppointmentWindow>()
            .ForMember(destination => destination.Id, property => property.MapFrom(source => new Guid(source.Id)));
    }
}

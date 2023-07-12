using AutoMapper;
using CCW.Schedule.Entities;
using CCW.Schedule.Models;

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

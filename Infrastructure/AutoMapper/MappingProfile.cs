using AutoMapper;
using Core.Entities;
using Infrastructure.DTOs.TaskItems;

namespace Infrastructure.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskItem, TaskItemDTO>()
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.UserName))
            .ForMember(dest => dest.UpdatedByName, opt => opt.MapFrom(src => src.UpdatedBy != null ? src.UpdatedBy.UserName : null))
            .ReverseMap();

        CreateMap<TaskItem, AddTaskItemDTO>().ReverseMap();
        CreateMap<TaskItem, UpdateTaskItemDTO>().ReverseMap();
    }
}
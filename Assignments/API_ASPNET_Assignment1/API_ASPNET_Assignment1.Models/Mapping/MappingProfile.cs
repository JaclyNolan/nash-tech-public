using API_ASPNET_Assignment1.Models.DTOs;
using API_ASPNET_Assignment1.Models.Entities;
using AutoMapper;

namespace API_ASPNET_Assignment1.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<TaskModel, TaskViewModel>();
            //CreateMap<List<TaskModel>, List<TaskViewModel>>();
            CreateMap<TaskRequestCreate, TaskModel>();
            //CreateMap<List<TaskRequestCreate>, List<TaskModel>>();
        }
    }
}

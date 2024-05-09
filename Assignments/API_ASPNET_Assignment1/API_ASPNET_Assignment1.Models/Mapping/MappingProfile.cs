using API_ASPNET_Assignment1.Models.DTOs;
using API_ASPNET_Assignment1.Models.Entities;
using API_ASPNET_Assignment1.WebAPI.DTOs;
using AutoMapper;

namespace API_ASPNET_Assignment1.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<TaskModel, TaskViewModel>();
            CreateMap<Person, PersonViewModel>();
            CreateMap<TaskRequestCreate, TaskModel>();
            CreateMap<PersonCreateRequest, Person>();
            CreateMap<PersonUpdateRequest, Person>();
        }
    }
}

using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Book;
using EF_Core_Assignment1.Application.DTOs.Common;
using EF_Core_Assignment1.Domain.Entities;

namespace EF_Core_Assignment1.Application.Mapper
{
    public class NashTechProfile : Profile
    {
        
        public NashTechProfile() {
            // Book
            CreateMap<CreateBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();
            CreateMap<Book, BookViewModel>();
        }
    }
}

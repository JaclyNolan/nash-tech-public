using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Account;
using EF_Core_Assignment1.Application.DTOs.Book;
using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequest;
using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequestDetails;
using EF_Core_Assignment1.Application.DTOs.Category;
using EF_Core_Assignment1.Application.DTOs.Role;
using EF_Core_Assignment1.Application.DTOs.User;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Identity;

namespace EF_Core_Assignment1.Application.Mapper
{
    public class NashTechProfile : Profile
    {

        public NashTechProfile()
        {
            // Book
            CreateMap<CreateBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();
            CreateMap<Book, BookViewModel>();

            // Category
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Category, CategoryViewModel>();

            // Account
            CreateMap<ApplicationRole, RoleViewModel>();
            CreateMap<ApplicationUser, AccountInfoViewModel>();

            // User 
            CreateMap<ApplicationUser, UserViewModel>();

            // Borrowing Request
            CreateMap<BookBorrowingRequest, BookBorrowingRequestAdminViewModel>();
            CreateMap<BookBorrowingRequest, BookBorrowingRequestUserViewModel>();

            // Borrowing Detail
            CreateMap<BookBorrowingRequestDetails, BorrowingDetailsViewModel>();
        }
    }
}

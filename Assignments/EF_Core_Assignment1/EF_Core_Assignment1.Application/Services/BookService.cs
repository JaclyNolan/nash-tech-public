using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Book;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.Services
{
    public interface IBookService
    {
        Task<(IEnumerable<BookViewAdminModel>, int totalCount)> GetAllBooksAsync(GetAllBookRequest request);
        Task<IEnumerable<BookViewAdminModel>> SearchBooksAsync(string searchString);
        Task<BookViewAdminModel?> GetBookByIdAsync(Guid id);
        Task<BookViewAdminModel> AddBookAsync(CreateBookRequest createBookRequest);
        Task<BookViewAdminModel> UpdateBookAsync(Guid id, UpdateBookRequest updateBookRequest);
        Task DeleteBookAsync(Guid id);
        Task<bool> IsBookExist(Guid id);
    }

    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<BookViewAdminModel>, int totalCount)> GetAllBooksAsync(GetAllBookRequest request)
        {
            var (books, totalCount) = await _bookRepository.GetBooksAsync(
                request.Page,
                request.PerPage,
                request.SortField.ToString(),
                request.SortOrder.ToString(),
                request.Search);
            var bookViewModels = _mapper.Map<IEnumerable<BookViewAdminModel>>(books);
            return (bookViewModels, totalCount);
        }

        public async Task<IEnumerable<BookViewAdminModel>> SearchBooksAsync(string searchString)
        {
            return _mapper.Map<List<BookViewAdminModel>>(await _bookRepository.SearchBooksAsync(searchString));
        }

        public async Task<BookViewAdminModel?> GetBookByIdAsync(Guid id)
        {
            return _mapper.Map<BookViewAdminModel>(await _bookRepository.GetByIdAsync(id));
        }

        public async Task<BookViewAdminModel> AddBookAsync(CreateBookRequest createBookRequest)
        {
            Book book = _mapper.Map<Book>(createBookRequest);
            return _mapper.Map<BookViewAdminModel>(await _bookRepository.AddAsync(book));
        }

        public async Task<BookViewAdminModel> UpdateBookAsync(Guid id, UpdateBookRequest updateBookRequest)
        {
            Book book = _mapper.Map<Book>(updateBookRequest);
            book.Id = id;
            return _mapper.Map<BookViewAdminModel>(await _bookRepository.UpdateAsync(book));
        }

        public async Task DeleteBookAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<bool> IsBookExist(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return book != null;
        }
    }
}

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
        Task<(IEnumerable<BookViewModel>, int totalCount)> GetAllBooksAsync(GetAllBookRequest request);
        Task<IEnumerable<BookViewModel>> SearchBooksAsync(string searchString);
        Task<BookViewModel?> GetBookByIdAsync(Guid id);
        Task<BookViewModel> AddBookAsync(CreateBookRequest createBookRequest);
        Task<BookViewModel> UpdateBookAsync(Guid id, UpdateBookRequest updateBookRequest);
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

        public async Task<(IEnumerable<BookViewModel>, int totalCount)> GetAllBooksAsync(GetAllBookRequest request)
        {
            var (books, totalCount) = await _bookRepository.GetBooksAsync(
                request.Page,
                request.PerPage,
                request.SortField.ToString(),
                request.SortOrder.ToString(),
                request.Search);
            var bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(books);
            return (bookViewModels, totalCount);
        }

        public async Task<IEnumerable<BookViewModel>> SearchBooksAsync(string searchString)
        {
            return _mapper.Map<List<BookViewModel>>(await _bookRepository.SearchBooksAsync(searchString));
        }

        public async Task<BookViewModel?> GetBookByIdAsync(Guid id)
        {
            return _mapper.Map<BookViewModel>(await _bookRepository.GetByIdAsync(id));
        }

        public async Task<BookViewModel> AddBookAsync(CreateBookRequest createBookRequest)
        {
            Book book = _mapper.Map<Book>(createBookRequest);
            return _mapper.Map<BookViewModel>(await _bookRepository.AddAsync(book));
        }

        public async Task<BookViewModel> UpdateBookAsync(Guid id, UpdateBookRequest updateBookRequest)
        {
            Book book = _mapper.Map<Book>(updateBookRequest);
            book.Id = id;
            return _mapper.Map<BookViewModel>(await _bookRepository.UpdateAsync(book));
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

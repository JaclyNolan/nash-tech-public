using EF_Core_Assignment1.Application.DTOs.Book;
using EF_Core_Assignment1.Application.DTOs.Common;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EF_Core_Assignment1.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<BookViewModel>>> GetAllBooks([FromQuery] GetAllBookRequest request)
        {

            var (books, totalCount) = await _bookService.GetAllBooksAsync(request);

            var paginatedResult = new PaginatedResult<BookViewModel>
            {
                Data = books,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Ok(paginatedResult);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookViewModel>>> SearchBooks(string query)
        {
            var books = await _bookService.SearchBooksAsync(query);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookViewModel>> GetBookById(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookViewModel>> AddBook([FromBody] CreateBookRequest createBookRequest)
        {
            var createdBook = await _bookService.AddBookAsync(createBookRequest);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookRequest updateBookRequest)
        {
            if (!await _bookService.IsBookExist(id))
            {
                return BadRequest();
            }

            await _bookService.UpdateBookAsync(id, updateBookRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}

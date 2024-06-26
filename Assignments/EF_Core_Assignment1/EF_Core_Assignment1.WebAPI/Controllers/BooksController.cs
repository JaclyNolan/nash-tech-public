﻿using EF_Core_Assignment1.Application.DTOs.Book;
using EF_Core_Assignment1.Application.DTOs.Common;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF_Core_Assignment1.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.User)}")]
        public async Task<ActionResult<PaginatedResult<BookViewAdminModel>>> GetAllBooks([FromQuery] GetAllBookRequest request)
        {
            var (books, totalCount) = await _bookService.GetAllBooksAsync(request);

            var paginatedResult = new PaginatedResult<BookViewAdminModel>
            {
                Data = books,
                PageNumber = request.Page,
                PageSize = request.PerPage,
                TotalCount = totalCount
            };

            return Ok(paginatedResult);
        }

        //[HttpGet("search")]
        //public async Task<ActionResult<IEnumerable<BookViewModel>>> SearchBooks(string query)
        //{
        //    var books = await _bookService.SearchBooksAsync(query);
        //    return Ok(books);
        //}

        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<ActionResult<BookViewAdminModel>> GetBookById(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<ActionResult<BookViewAdminModel>> AddBook([FromBody] CreateBookRequest createBookRequest)
        {
            var createdBook = await _bookService.AddBookAsync(createBookRequest);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
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
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}

using EF_Core_Assignment1.Application.DTOs.Book;
using EF_Core_Assignment1.Application.DTOs.Common;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EF_Core_Assignment1.WebAPI.Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BooksController _booksController;

        public BooksControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _booksController = new BooksController(_mockBookService.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsPaginatedResult()
        {
            // Arrange
            var request = new GetAllBookRequest { Page = 1, PerPage = 10 };
            var books = new List<BookViewAdminModel> { new BookViewAdminModel { Id = Guid.NewGuid(), Title = "Test Book" } };
            var totalCount = 1;
            _mockBookService.Setup(service => service.GetAllBooksAsync(request))
                .ReturnsAsync((books, totalCount));

            // Act
            var result = await _booksController.GetAllBooks(request);
            var okResult = result.Result as OkObjectResult;
            var paginatedResult = okResult.Value as PaginatedResult<BookViewAdminModel>;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<PaginatedResult<BookViewAdminModel>>(okResult.Value);
            Assert.Equal(books, paginatedResult.Data);
            Assert.Equal(totalCount, paginatedResult.TotalCount);
            Assert.Equal(request.Page, paginatedResult.PageNumber);
            Assert.Equal(request.PerPage, paginatedResult.PageSize);
        }

        [Fact]
        public async Task GetBookById_BookExists_ReturnsBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new BookViewAdminModel { Id = bookId, Title = "Test Book" };
            _mockBookService.Setup(service => service.GetBookByIdAsync(bookId))
                .ReturnsAsync(book);

            // Act
            var result = await _booksController.GetBookById(bookId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(book, okResult.Value);
        }

        [Fact]
        public async Task GetBookById_BookDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _mockBookService.Setup(service => service.GetBookByIdAsync(bookId))
                .ReturnsAsync((BookViewAdminModel?)null);

            // Act
            var result = await _booksController.GetBookById(bookId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddBook_ValidRequest_ReturnsCreatedBook()
        {
            // Arrange
            var createBookRequest = new CreateBookRequest { Title = "New Book" };
            var createdBook = new BookViewAdminModel { Id = Guid.NewGuid(), Title = "New Book" };
            _mockBookService.Setup(service => service.AddBookAsync(createBookRequest))
                .ReturnsAsync(createdBook);

            // Act
            var result = await _booksController.AddBook(createBookRequest);
            var createdResult = result.Result as CreatedAtActionResult;

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResult);
            Assert.NotNull(createdResult);
            Assert.Equal(nameof(_booksController.GetBookById), createdResult.ActionName);
            Assert.Equal(createdBook.Id, createdResult.RouteValues["id"]);
            Assert.Equal(createdBook, createdResult.Value);
        }

        [Fact]
        public async Task UpdateBook_BookExists_ReturnsNoContent()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var updateBookRequest = new UpdateBookRequest { Title = "Updated Title" };
            _mockBookService.Setup(service => service.IsBookExist(bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _booksController.UpdateBook(bookId, updateBookRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateBook_BookDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var updateBookRequest = new UpdateBookRequest { Title = "Updated Title" };
            _mockBookService.Setup(service => service.IsBookExist(bookId))
                .ReturnsAsync(false);

            // Act
            var result = await _booksController.UpdateBook(bookId, updateBookRequest);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContent()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Act
            var result = await _booksController.DeleteBook(bookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

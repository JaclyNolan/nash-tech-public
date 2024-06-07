using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Book;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Repositories;
using Moq;

namespace EF_Core_Assignment1.Application.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _mockMapper = new Mock<IMapper>();
            _bookService = new BookService(_mockBookRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsBooksAndTotalCount()
        {
            // Arrange
            var request = new GetAllBookRequest { Page = 1, PerPage = 10 };
            var books = new List<Book> { new Book { Id = Guid.NewGuid(), Title = "Test Book" } };
            var totalCount = 1;

            _mockBookRepository.Setup(repo => repo.GetBooksAsync(
                request.Page,
                request.PerPage,
                request.SortField.ToString(),
                request.SortOrder.ToString(),
                request.Search))
                .ReturnsAsync((books, totalCount));

            _mockMapper.Setup(m => m.Map<IEnumerable<BookViewAdminModel>>(books))
                .Returns(new List<BookViewAdminModel> { new BookViewAdminModel { Id = Guid.NewGuid(), Title = "Test Book" } });

            // Act
            var result = await _bookService.GetAllBooksAsync(request);

            // Assert
            Assert.Equal(1, result.totalCount);
            Assert.NotEmpty(result.Item1);
            _mockBookRepository.Verify(repo => repo.GetBooksAsync(request.Page, request.PerPage, request.SortField.ToString(), request.SortOrder.ToString(), request.Search), Times.Once);
        }

        [Fact]
        public async Task SearchBooksAsync_ReturnsMatchingBooks()
        {
            // Arrange
            var searchString = "Test";
            var books = new List<Book> { new Book { Id = Guid.NewGuid(), Title = "Test Book" } };

            _mockBookRepository.Setup(repo => repo.SearchBooksAsync(searchString))
                .ReturnsAsync(books);

            _mockMapper.Setup(m => m.Map<List<BookViewAdminModel>>(books))
                .Returns(new List<BookViewAdminModel> { new BookViewAdminModel { Id = Guid.NewGuid(), Title = "Test Book" } });

            // Act
            var result = await _bookService.SearchBooksAsync(searchString);

            // Assert
            Assert.NotEmpty(result);
            _mockBookRepository.Verify(repo => repo.SearchBooksAsync(searchString), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsBook_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId, Title = "Test Book" };

            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(book);

            _mockMapper.Setup(m => m.Map<BookViewAdminModel>(book))
                .Returns(new BookViewAdminModel { Id = bookId, Title = "Test Book" });

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task AddBookAsync_ReturnsCreatedBook()
        {
            // Arrange
            var createBookRequest = new CreateBookRequest { Title = "New Book" };
            var book = new Book { Id = Guid.NewGuid(), Title = "New Book" };

            _mockMapper.Setup(m => m.Map<Book>(createBookRequest))
                .Returns(book);

            _mockBookRepository.Setup(repo => repo.AddAsync(book))
                .ReturnsAsync(book);

            _mockMapper.Setup(m => m.Map<BookViewAdminModel>(book))
                .Returns(new BookViewAdminModel { Id = book.Id, Title = "New Book" });

            // Act
            var result = await _bookService.AddBookAsync(createBookRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            _mockBookRepository.Verify(repo => repo.AddAsync(book), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_ReturnsUpdatedBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var updateBookRequest = new UpdateBookRequest { Title = "Updated Book" };
            var book = new Book { Id = bookId, Title = "Updated Book" };

            _mockMapper.Setup(m => m.Map<Book>(updateBookRequest))
                .Returns(book);

            _mockBookRepository.Setup(repo => repo.UpdateAsync(book))
                .ReturnsAsync(book);

            _mockMapper.Setup(m => m.Map<BookViewAdminModel>(book))
                .Returns(new BookViewAdminModel { Id = bookId, Title = "Updated Book" });

            // Act
            var result = await _bookService.UpdateBookAsync(bookId, updateBookRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
            _mockBookRepository.Verify(repo => repo.UpdateAsync(book), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_DeletesBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            _mockBookRepository.Setup(repo => repo.DeleteAsync(bookId))
                .Returns(Task.CompletedTask);

            // Act
            await _bookService.DeleteBookAsync(bookId);

            // Assert
            _mockBookRepository.Verify(repo => repo.DeleteAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task IsBookExist_ReturnsTrue_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId, Title = "Test Book" };

            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(book);

            // Act
            var result = await _bookService.IsBookExist(bookId);

            // Assert
            Assert.True(result);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task IsBookExist_ReturnsFalse_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.IsBookExist(bookId);

            // Assert
            Assert.False(result);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
        }
    }
}

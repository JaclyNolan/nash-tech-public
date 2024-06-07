using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequest;
using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequestDetails;
using EF_Core_Assignment1.Application.DTOs.Common;
using EF_Core_Assignment1.Application.Exceptions;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Configurations;
using EF_Core_Assignment1.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace EF_Core_Assignment1.WebAPI.Tests.Controllers
{
    public class BorrowingRequestsControllerTests
    {
        private readonly Mock<IBorrowingRequestService> _mockBorrowingRequestService;
        private readonly BorrowingRequestsController _controller;
        private readonly ClaimsPrincipal _user;
        private readonly BorrowingSettings _borrowingSettings;

        public BorrowingRequestsControllerTests()
        {
            _mockBorrowingRequestService = new Mock<IBorrowingRequestService>();

            // Mock the user
            _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, nameof(UserRole.Admin))
            }, "mock"));

            _borrowingSettings = new BorrowingSettings
            {
                MaxBooksPerRequest = 5,
                MaxRequestsPerMonth = 3
            };
            ApplicationSettings.BorrowingSettings = _borrowingSettings;

            _controller = new BorrowingRequestsController(_mockBorrowingRequestService.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = _user }
            };
        }

        [Fact]
        public async Task GetAllBorrowingRequest_ReturnsPaginatedResult()
        {
            // Arrange
            var request = new GetAllBorrowingRequest { Page = 1, PerPage = 10 };
            var borrowingRequests = new List<BookBorrowingRequestAdminViewModel>
                {
                    new BookBorrowingRequestAdminViewModel { Id = Guid.NewGuid(), Status = BookRequestStatus.Approved }
                };
            var totalCount = 1;

            _mockBorrowingRequestService.Setup(service => service.GetAllBorrowingRequestAsync(request))
                .ReturnsAsync((borrowingRequests, totalCount));

            // Act
            var result = await _controller.GetAllBorrowingRequest(request);
            var okResult = result.Result as OkObjectResult;
            var paginatedResult = okResult.Value as PaginatedResult<BookBorrowingRequestAdminViewModel>;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<PaginatedResult<BookBorrowingRequestAdminViewModel>>(okResult.Value);
            Assert.Equal(borrowingRequests, paginatedResult.Data);
            Assert.Equal(totalCount, paginatedResult.TotalCount);
            Assert.Equal(request.Page, paginatedResult.PageNumber);
            Assert.Equal(request.PerPage, paginatedResult.PageSize);
        }

        [Fact]
        public async Task UpdateStatus_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var request = new UpdateBorrowingRequest { Status = BookRequestStatus.Approved };

            _mockBorrowingRequestService.Setup(service => service.UpdateRequestStatus(
                It.IsAny<Guid>(),
                It.IsAny<BookRequestStatus>(),
                It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateStatus(requestId, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateStatus_NotFoundException_ReturnsNotFound()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var request = new UpdateBorrowingRequest { Status = BookRequestStatus.Approved };

            _mockBorrowingRequestService.Setup(service => service.UpdateRequestStatus(
                It.IsAny<Guid>(),
                It.IsAny<BookRequestStatus>(),
                It.IsAny<Guid>()))
                .ThrowsAsync(new NotFoundException("Request not found"));

            // Act
            var result = await _controller.UpdateStatus(requestId, request);
            var notFoundResult = result as NotFoundObjectResult;

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
            Assert.Equal(new { message = "Request not found" }.ToString(), notFoundResult.Value.ToString());
        }

        [Fact]
        public async Task CreateUserRequest_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new CreateBorrowingUserRequest
            {
                RequestDetails = new List<CreateBorrowingRequestDetailUserRequest>
                {
                    new CreateBorrowingRequestDetailUserRequest { BookId = Guid.NewGuid(), BorrowedDate = DateOnly.FromDateTime(DateTime.Now), ReturnedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) }
                }
            };

            _mockBorrowingRequestService.Setup(service => service.GetBorrowingRequestForUserThisMonth(It.IsAny<string>()))
                .ReturnsAsync(new List<BookBorrowingRequestUserViewModel>());

            _mockBorrowingRequestService.Setup(service => service.CreateUserBookBorrowingRequest(It.IsAny<string>(), request))
                .Returns(Task.CompletedTask);

            // Act
            var applicationSet = ApplicationSettings.BorrowingSettings;
            var result = await _controller.CreateUserRequest(request);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Request's made!", okResult.Value);
        }

        [Fact]
        public async Task CreateUserRequest_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateBorrowingUserRequest { RequestDetails = null };

            // Act
            var result = await _controller.CreateUserRequest(request);
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.Equal("Invalid request. Please provide borrowing details.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateUserRequest_ExceedsMaxBooksPerRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateBorrowingUserRequest
            {
                RequestDetails = new List<CreateBorrowingRequestDetailUserRequest>(new CreateBorrowingRequestDetailUserRequest[ApplicationSettings.BorrowingSettings.MaxBooksPerRequest + 1])
            };

            // Act
            var result = await _controller.CreateUserRequest(request);
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            //Assert.Equal($"You can borrow up to {ApplicationSettings.BorrowingSettings.MaxBooksPerRequest} books in one request.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateUserRequest_ExceedsMonthlyLimit_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateBorrowingUserRequest
            {
                RequestDetails = new List<CreateBorrowingRequestDetailUserRequest>
                {
                    new CreateBorrowingRequestDetailUserRequest { BookId = Guid.NewGuid(), BorrowedDate = DateOnly.FromDateTime(DateTime.UtcNow), ReturnedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) }
                }
            };

            var borrowingRequests = new List<BookBorrowingRequestUserViewModel>(new BookBorrowingRequestUserViewModel[ApplicationSettings.BorrowingSettings.MaxRequestsPerMonth + 1]);
            _mockBorrowingRequestService.Setup(service => service.GetBorrowingRequestForUserThisMonth(It.IsAny<string>())).ReturnsAsync(borrowingRequests);

            // Act
            var result = await _controller.CreateUserRequest(request);
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            //Assert.Equal($"You have reached the monthly borrowing request limit of {ApplicationSettings.BorrowingSettings.MaxRequestsPerMonth}.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateUserRequest_ValidationException_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateBorrowingUserRequest
            {
                RequestDetails = new List<CreateBorrowingRequestDetailUserRequest>
                {
                    new CreateBorrowingRequestDetailUserRequest { BookId = Guid.NewGuid(), BorrowedDate = DateOnly.FromDateTime(DateTime.UtcNow), ReturnedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) }
                }
            };

            var borrowingRequests = new List<BookBorrowingRequestUserViewModel>(new BookBorrowingRequestUserViewModel[ApplicationSettings.BorrowingSettings.MaxRequestsPerMonth]);
            _mockBorrowingRequestService.Setup(service => service.GetBorrowingRequestForUserThisMonth(It.IsAny<string>())).ReturnsAsync(borrowingRequests);
            _mockBorrowingRequestService.Setup(service => service.CreateUserBookBorrowingRequest(It.IsAny<string>(), request))
                .ThrowsAsync(new BorrowingValidationException("Validation error message"));

            // Act
            var result = await _controller.CreateUserRequest(request);
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            //Assert.Equal("Validation error: Validation error message", badRequestResult.Value);
        }

        [Fact]
        public async Task GetRequestForUserThisMonth_ReturnsUserRequests()
        {
            // Arrange
            var userRequests = new List<BookBorrowingRequestUserViewModel>
        {
            new BookBorrowingRequestUserViewModel { Id = Guid.NewGuid(), Status = BookRequestStatus.Approved }
        };

            _mockBorrowingRequestService.Setup(service => service.GetBorrowingRequestForUserThisMonth(It.IsAny<string>()))
                .ReturnsAsync(userRequests);

            // Act
            var result = await _controller.GetRequestForUserThisMonth();
            var okResult = result.Result as OkObjectResult;
            var returnedRequests = okResult.Value as List<BookBorrowingRequestUserViewModel>;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(userRequests, returnedRequests);
        }

        [Fact]
        public async Task GetRequestForUser_ReturnsUserRequests()
        {
            // Arrange
            var userRequests = new List<BookBorrowingRequestUserViewModel>
        {
            new BookBorrowingRequestUserViewModel { Id = Guid.NewGuid(), Status = BookRequestStatus.Approved }
        };

            _mockBorrowingRequestService.Setup(service => service.GetBorrowingRequestForUser(It.IsAny<string>()))
                .ReturnsAsync(userRequests);

            // Act
            var result = await _controller.GetRequestForUser();
            var okResult = result.Result as OkObjectResult;
            var returnedRequests = okResult.Value as List<BookBorrowingRequestUserViewModel>;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(userRequests, returnedRequests);
        }
    }
}

using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequest;
using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequestDetails;
using EF_Core_Assignment1.Application.Exceptions;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Repositories;
using Moq;

namespace EF_Core_Assignment1.Application.Tests.Services
{
    public class BorrowingRequestServiceTests
    {
        private readonly Mock<IBorrowingRequestRepository> _mockBorrowingRequestRepository;
        private readonly Mock<IBorrowingRequestDetailRepository> _mockBorrowingRequestDetailRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BorrowingRequestService _service;

        public BorrowingRequestServiceTests()
        {
            _mockBorrowingRequestRepository = new Mock<IBorrowingRequestRepository>();
            _mockBorrowingRequestDetailRepository = new Mock<IBorrowingRequestDetailRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new BorrowingRequestService(
                _mockBorrowingRequestRepository.Object,
                _mockBorrowingRequestDetailRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetAllBorrowingRequestAsync_ReturnsPaginatedResult()
        {
            // Arrange
            var request = new GetAllBorrowingRequest { Page = 1, PerPage = 10 };
            var borrowingRequests = new List<BookBorrowingRequest> { new BookBorrowingRequest { Id = Guid.NewGuid() } };
            var totalCount = 1;

            _mockBorrowingRequestRepository.Setup(repo => repo.GetBorrowingRequestsAsync(
                request.Page,
                request.PerPage,
                request.SortField.ToString(),
                request.SortOrder.ToString()))
                .ReturnsAsync((borrowingRequests, totalCount));

            var borrowingRequestViewModels = new List<BookBorrowingRequestAdminViewModel>
        {
            new BookBorrowingRequestAdminViewModel { Id = Guid.NewGuid() }
        };

            _mockMapper.Setup(m => m.Map<IEnumerable<BookBorrowingRequestAdminViewModel>>(borrowingRequests))
                .Returns(borrowingRequestViewModels);

            // Act
            var result = await _service.GetAllBorrowingRequestAsync(request);

            // Assert
            Assert.Equal(borrowingRequestViewModels, result.Item1);
            Assert.Equal(totalCount, result.Item2);
        }

        [Fact]
        public async Task UpdateRequestStatus_ValidRequest_UpdatesStatus()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var actionerId = Guid.NewGuid();
            var status = BookRequestStatus.Approved;
            var borrowingRequest = new BookBorrowingRequest { Id = requestId };

            _mockBorrowingRequestRepository.Setup(repo => repo.GetByIdAsync(requestId))
                .ReturnsAsync(borrowingRequest);

            _mockBorrowingRequestRepository.Setup(repo => repo.UpdateAsync(borrowingRequest))
                .ReturnsAsync(borrowingRequest);

            // Act
            await _service.UpdateRequestStatus(requestId, status, actionerId);

            // Assert
            Assert.Equal(status, borrowingRequest.Status);
            Assert.Equal(actionerId.ToString(), borrowingRequest.ActionerId);
            _mockBorrowingRequestRepository.Verify(repo => repo.UpdateAsync(borrowingRequest), Times.Once);
        }

        [Fact]
        public async Task UpdateRequestStatus_RequestNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var actionerId = Guid.NewGuid();
            var status = BookRequestStatus.Approved;

            _mockBorrowingRequestRepository.Setup(repo => repo.GetByIdAsync(requestId))
                .ReturnsAsync((BookBorrowingRequest)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateRequestStatus(requestId, status, actionerId));
        }

        [Fact]
        public async Task CreateUserBookBorrowingRequest_ValidRequest_CreatesRequest()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new CreateBorrowingUserRequest
            {
                RequestDetails = new List<CreateBorrowingRequestDetailUserRequest>
            {
                new CreateBorrowingRequestDetailUserRequest { BookId = Guid.NewGuid(), BorrowedDate = DateOnly.FromDateTime(DateTime.UtcNow), ReturnedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) }
            }
            };

            _mockBorrowingRequestRepository.Setup(repo => repo.AddAsync(It.IsAny<BookBorrowingRequest>()))
                .Returns(Task.FromResult(new BookBorrowingRequest()));

            // Act
            await _service.CreateUserBookBorrowingRequest(userId, request);

            // Assert
            _mockBorrowingRequestRepository.Verify(repo => repo.AddAsync(It.IsAny<BookBorrowingRequest>()), Times.Once);
        }

        [Fact]
        public async Task CreateUserBookBorrowingRequest_InvalidDates_ThrowsBorrowedDateMustBeBeforeReturnedException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new CreateBorrowingUserRequest
            {
                RequestDetails = new List<CreateBorrowingRequestDetailUserRequest>
            {
                new CreateBorrowingRequestDetailUserRequest { BookId = Guid.NewGuid(), BorrowedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), ReturnedDate = DateOnly.FromDateTime(DateTime.UtcNow) }
            }
            };

            // Act & Assert
            await Assert.ThrowsAsync<BorrowedDateMustBeBeforeReturnedException>(() => _service.CreateUserBookBorrowingRequest(userId, request));
        }

        [Fact]
        public async Task CreateUserBookBorrowingRequest_DuplicateBookIds_ThrowsDuplicateBookIdException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var bookId = Guid.NewGuid();
            var request = new CreateBorrowingUserRequest
            {
                RequestDetails = new List<CreateBorrowingRequestDetailUserRequest>
            {
                new CreateBorrowingRequestDetailUserRequest { BookId = bookId, BorrowedDate = DateOnly.FromDateTime(DateTime.UtcNow), ReturnedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) },
                new CreateBorrowingRequestDetailUserRequest { BookId = bookId, BorrowedDate = DateOnly.FromDateTime(DateTime.UtcNow), ReturnedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) }
            }
            };

            // Act & Assert
            await Assert.ThrowsAsync<DuplicateBookIdException>(() => _service.CreateUserBookBorrowingRequest(userId, request));
        }

        [Fact]
        public async Task GetBorrowingRequestForUserThisMonth_ReturnsRequests()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var borrowingRequests = new List<BookBorrowingRequestUserViewModel>
        {
            new BookBorrowingRequestUserViewModel { Id = Guid.NewGuid() }
        };

            _mockBorrowingRequestRepository.Setup(repo => repo.GetBorrowingRequestForUserBetween(userId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<BookBorrowingRequest>());

            _mockMapper.Setup(m => m.Map<List<BookBorrowingRequestUserViewModel>>(It.IsAny<List<BookBorrowingRequest>>()))
                .Returns(borrowingRequests);

            // Act
            var result = await _service.GetBorrowingRequestForUserThisMonth(userId);

            // Assert
            Assert.Equal(borrowingRequests, result);
        }

        [Fact]
        public async Task GetBorrowingRequestForUser_ReturnsRequests()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var borrowingRequests = new List<BookBorrowingRequestUserViewModel>
        {
            new BookBorrowingRequestUserViewModel { Id = Guid.NewGuid() }
        };

            _mockBorrowingRequestRepository.Setup(repo => repo.GetBorrowingRequestForUser(userId))
                .ReturnsAsync(new List<BookBorrowingRequest>());

            _mockMapper.Setup(m => m.Map<List<BookBorrowingRequestUserViewModel>>(It.IsAny<List<BookBorrowingRequest>>()))
                .Returns(borrowingRequests);

            // Act
            var result = await _service.GetBorrowingRequestForUser(userId);

            // Assert
            Assert.Equal(borrowingRequests, result);
        }
    }
}

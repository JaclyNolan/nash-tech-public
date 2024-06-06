using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequest;
using EF_Core_Assignment1.Application.Exceptions;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Repositories;

namespace EF_Core_Assignment1.Application.Services
{
    public interface IBorrowingRequestService
    {
        Task<(IEnumerable<BookBorrowingRequestAdminViewModel>, int totalCount)> GetAllBorrowingRequestAsync(GetAllBorrowingRequest request);
        Task UpdateRequestStatus(Guid id, BookRequestStatus status, Guid actionerId);
        Task CreateUserBookBorrowingRequest(string userId, CreateBorrowingUserRequest request);
        Task<List<BookBorrowingRequestUserViewModel>> GetBorrowingRequestForUser(string userId);
        Task<List<BookBorrowingRequestUserViewModel>> GetBorrowingRequestForUserThisMonth(string userId);
        Task<List<BookBorrowingRequestUserViewModel>> GetBorrowingRequestForUserBetween(string userId, DateTime startDate, DateTime endDate);
    }
    public class BorrowingRequestService : IBorrowingRequestService
    {
        private readonly IBorrowingRequestRepository _borrowingRequestRepository;
        private readonly IBorrowingRequestDetailRepository _borrowingRequestDetailRepository;
        private readonly IMapper _mapper;

        public BorrowingRequestService(IBorrowingRequestRepository borrowingRequestRepository, IBorrowingRequestDetailRepository borrowingRequestDetailRepository, IMapper mapper)
        {
            _borrowingRequestRepository = borrowingRequestRepository;
            _borrowingRequestDetailRepository = borrowingRequestDetailRepository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<BookBorrowingRequestAdminViewModel>, int totalCount)> GetAllBorrowingRequestAsync(GetAllBorrowingRequest request)
        {
            var (borrowingRequests, totalCount) = await _borrowingRequestRepository.GetBorrowingRequestsAsync(
                request.Page,
                request.PerPage,
                request.SortField.ToString(),
                request.SortOrder.ToString());
            var borrowingRequestViewModels = _mapper.Map<IEnumerable<BookBorrowingRequestAdminViewModel>>(borrowingRequests);
            return (borrowingRequestViewModels, totalCount);
        }

        public async Task UpdateRequestStatus(Guid id, BookRequestStatus status, Guid actionerId)
        {
            var request = await _borrowingRequestRepository.GetByIdAsync(id);
            if (request == null)
            {
                throw new NotFoundException($"BookBorrowingRequest with id {id} not found.");
            }
            if (status == BookRequestStatus.Approved || status == BookRequestStatus.Rejected)
            {
                request.ActionerId = actionerId.ToString();
            }
            request.Status = status;
            await _borrowingRequestRepository.UpdateAsync(request);
        }

        public async Task CreateUserBookBorrowingRequest(string userId, CreateBorrowingUserRequest request)
        {
            // Create borrowing requests
            var borrowingRequest = new BookBorrowingRequest
            {
                RequestorId = userId,
                RequestDate = DateTime.Now,
                Status = BookRequestStatus.Waiting,
            };

            var borrowingRequestDetails = new List<BookBorrowingRequestDetails>();
            foreach (var detail in request.RequestDetails)
            {
                var borrowingRequestDetail = new BookBorrowingRequestDetails
                {
                    BookId = detail.BookId,
                    BookBorrowingRequestId = borrowingRequest.Id,
                    ReturnedDate = detail.ReturnedDate.ToDateTime(new TimeOnly(0,0,0)),
                    BorrowedDate = detail.BorrowedDate.ToDateTime(new TimeOnly(0,0,0))
                };
                borrowingRequestDetails.Add(borrowingRequestDetail);
            }

            // Make a unit of work pattern or make a generic repo to do save change when dealing with changing two tables at the same time
            // To-do: implement the stuff above after everything is done.
            await _borrowingRequestDetailRepository.AddManyAsync(borrowingRequestDetails);
            await _borrowingRequestRepository.AddAsync(borrowingRequest);
        }

        public async Task<List<BookBorrowingRequestUserViewModel>> GetBorrowingRequestForUserThisMonth(string userId)
        {
            var currentDate = DateTime.UtcNow;
            var startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            return await GetBorrowingRequestForUserBetween(userId, startDate, endDate);
        }
        public async Task<List<BookBorrowingRequestUserViewModel>> GetBorrowingRequestForUser(string userId)
        {
            var borrowingRequests = await _borrowingRequestRepository.GetBorrowingRequestForUser(userId);
            return _mapper.Map<List<BookBorrowingRequestUserViewModel>>(borrowingRequests);
        }

        public async Task<List<BookBorrowingRequestUserViewModel>> GetBorrowingRequestForUserBetween(string userId, DateTime startDate, DateTime endDate)
        {
            var borrowingRequests = await _borrowingRequestRepository.GetBorrowingRequestForUserBetween(userId, startDate, endDate);
            return _mapper.Map<List<BookBorrowingRequestUserViewModel>>(borrowingRequests);
        }
    }
}

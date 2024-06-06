using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequestDetails;
using EF_Core_Assignment1.Application.DTOs.User;
using EF_Core_Assignment1.Domain.Entities;

namespace EF_Core_Assignment1.Application.DTOs.BookBorrowingRequest
{
    public class BookBorrowingRequestAdminViewModel
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public BookRequestStatus Status { get; set; } = BookRequestStatus.Waiting;
        public string RequestorId { get; set; }
        public string ActionerId { get; set; }

        //Navigation property view models
        public UserViewModel Requestor { get; set; }
        public UserViewModel Actioner { get; set; }
        public ICollection<BorrowingDetailsViewModel> BookBorrowingRequestDetails { get; set; } = new List<BorrowingDetailsViewModel>();
    }

    public class BookBorrowingRequestUserViewModel
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public BookRequestStatus Status { get; set; }
        public ICollection<BorrowingDetailsViewModel> BookBorrowingRequestDetails { get; set; } = new List<BorrowingDetailsViewModel>();
    }
}

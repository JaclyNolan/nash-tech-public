using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequestDetails;
using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Application.DTOs.BookBorrowingRequest
{
    public class CreateBorrowingUserRequest
    {
        [Required]
        public ICollection<CreateBorrowingRequestDetailUserRequest> RequestDetails { get; set; } // Details of the books being requested
    }
}

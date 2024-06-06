using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Application.DTOs.BookBorrowingRequestDetails
{
    public class CreateBorrowingRequestDetailUserRequest
    {
        [Required]
        public Guid BookId { get; set; }
        [Required]
        public DateOnly BorrowedDate { get; set; }
        [Required]
        public DateOnly ReturnedDate { get; set; }
    }
}

using EF_Core_Assignment1.Domain.Entities;

namespace EF_Core_Assignment1.Application.DTOs.BookBorrowingRequest
{
    public class UpdateBorrowingRequest
    {
        public BookRequestStatus Status { get; set; }
    }
}

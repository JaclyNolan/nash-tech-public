using EF_Core_Assignment1.Application.DTOs.Book;

namespace EF_Core_Assignment1.Application.DTOs.BookBorrowingRequestDetails
{
    public class BorrowingDetailsViewModel
    {
        public Guid Id { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        public Guid BookId { get; set; }
        public Guid BookBorrowingRequestId { get; set; }
        public BookViewModel Book { get; set; }
    }
}

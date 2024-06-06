using EF_Core_Assignment1.Application.DTOs.Book;

namespace EF_Core_Assignment1.Application.DTOs.BookBorrowingRequestDetails
{
    public class BorrowingDetailsAdminViewModel
    {
        public Guid Id { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        public Guid BookId { get; set; }
        public Guid BookBorrowingRequestId { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public BookViewAdminModel Book { get; set; }
    }

    public class BorrowingDetailsUserViewModel
    {
        public Guid Id { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        public Guid BookId { get; set; }
        public Guid BookBorrowingRequestId { get; set; }
        public BookUserViewModel Book { get; set; }
    }
}

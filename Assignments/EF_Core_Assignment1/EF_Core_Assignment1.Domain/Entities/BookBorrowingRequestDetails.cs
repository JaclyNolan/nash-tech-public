using EF_Core_Assignment1.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class BookBorrowingRequestDetails : BaseEntity
    {
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        public Guid BookBorrowingRequestId { get; set; }

        public Guid BookId { get; set; }

        [Required]
        public DateTime BorrowedDate { get; set; }

        [Required]
        public DateTime ReturnedDate { get; set; }

        // Navigation properties
        public BookBorrowingRequest BookBorrowingRequest { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}

using EF_Core_Assignment1.Domain.Common;
using EF_Core_Assignment1.Persistance.Identity;
using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class BookBorrowingRequest : BaseEntity
    {
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        public string RequestorId { get; set; } = null!;

        public string? ApproverId { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        public BookRequestStatus Status { get; set; } = BookRequestStatus.Waiting;

        // Navigation property
        public ApplicationUser Requestor { get; set; } = null!;
        public ApplicationUser? Approver { get; set; }
        public ICollection<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; } = new List<BookBorrowingRequestDetails>();
    }

    public enum BookRequestStatus {
        Waiting = 0,
        Rejected = 1,
        Approved = 2
    }
}

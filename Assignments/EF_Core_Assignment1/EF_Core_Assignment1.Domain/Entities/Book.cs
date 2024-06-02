using EF_Core_Assignment1.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class Book : BaseEntity
    {
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(200, ErrorMessage = "Title length can't be more than 200 characters.")]
        public string Title { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Author length can't be more than 200 characters.")]
        public string? Author { get; set; }

        [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
        public string? Description { get; set; }

        public Guid? CategoryId { get; set; }

        // Navigation property
        public Category? Category { get; set; }
        public ICollection<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; } = new List<BookBorrowingRequestDetails>();
    }
}
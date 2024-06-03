using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Application.DTOs.Book
{
    public class UpdateBookRequest
    {
        [Required]
        [StringLength(200, ErrorMessage = "Title length can't be more than 200 characters.")]
        public string Title { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Author length can't be more than 200 characters.")]
        public string? Author { get; set; }

        [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
        public string? Description { get; set; }

        public Guid? CategoryId { get; set; }
    }
}

using EF_Core_Assignment1.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(200, ErrorMessage = "Category name length can't be more than 200 characters.")]
        public required string Name { get; set; }

        // Navigation property
        public ICollection<Book>? Books { get; set; }
    }
}

using EF_Core_Assignment1.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class Book : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
        public string Description { get; set; }
    }
}
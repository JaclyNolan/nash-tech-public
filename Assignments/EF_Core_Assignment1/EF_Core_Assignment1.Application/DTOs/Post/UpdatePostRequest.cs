using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Application.DTOs.Post
{
    public class UpdatePostRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Body length can't be more than 500 characters.")]
        public string Body { get; set; }
    }
}

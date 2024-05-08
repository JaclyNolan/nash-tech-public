using System.ComponentModel.DataAnnotations;

namespace API_ASPNET_Assignment1.Models.DTOs
{
    public class TaskViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
    }
}

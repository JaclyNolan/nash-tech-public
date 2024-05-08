using System.ComponentModel.DataAnnotations;

namespace API_ASPNET_Assignment1.Models.Entities
{
    public class TaskModel
    {
        public TaskModel()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public bool IsCompleted { get; set; } = false;
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_ASPNET_Assignment1.Models.DTOs
{
    public class TaskRequestCreate
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public bool IsCompleted { get; set; }

    }
}

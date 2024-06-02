using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.DTOs.Category
{
    public class UpdateCategoryRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; }
    }
}

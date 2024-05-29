﻿using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Application.DTOs.Common
{
    public class UpdateBookRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}

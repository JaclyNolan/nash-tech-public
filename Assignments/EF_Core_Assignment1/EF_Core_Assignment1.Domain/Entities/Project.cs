using EF_Core_Assignment1.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }

    }
}

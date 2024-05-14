using EF_Core_Assignment1.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class ProjectEmployee : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public Project Project { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        [Required]
        public bool Enable { get; set; }
    }
}

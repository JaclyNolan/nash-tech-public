using EF_Core_Assignment1.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class Employee : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoinedDate { get; set; }
        // One-to-one Employee Salary
        public Salary? Salary { get; set; } = null;
        // One-to-many Employee Department
        [Required]
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    }
}

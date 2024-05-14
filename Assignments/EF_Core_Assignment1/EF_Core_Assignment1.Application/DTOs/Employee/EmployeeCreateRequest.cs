using EF_Core_Assignment1.Application.DTOs.Salary;
using EF_Core_Assignment1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.DTOs.Employee
{
    public class EmployeeCreateRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoinedDate { get; set; }
        // One-to-one Employee Salary
        [Required]
        public float SalaryAmount { get; set; }
        // One-to-many Employee Department
        [Required]
        public Guid DepartmentId { get; set; }
    }
}

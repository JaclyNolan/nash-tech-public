using Entities = EF_Core_Assignment1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_Core_Assignment1.Application.DTOs.Salary;
using EF_Core_Assignment1.Domain.Entities;

namespace EF_Core_Assignment1.Application.DTOs.Employee
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime JoinedDate { get; set; }
        // One-to-one Employee Salary
        public float SalaryAmount { get; set; }
        // One-to-many Employee Department
        public Guid DepartmentId { get; set; }
        public ICollection<Guid> ProjectIds { get; set; }
    }
}

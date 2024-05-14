using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.DTOs.Salary
{
    public class SalaryViewModel
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public float SalaryAmount { get; set; }
    }
}

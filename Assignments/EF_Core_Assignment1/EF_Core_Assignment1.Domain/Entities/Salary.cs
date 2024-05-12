using EF_Core_Assignment1.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class Salary : BaseEntity
    {
        [Required]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Required]
        public float SalaryAmmount { get; set; }
    }
}

using EF_Core_Assignment1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}

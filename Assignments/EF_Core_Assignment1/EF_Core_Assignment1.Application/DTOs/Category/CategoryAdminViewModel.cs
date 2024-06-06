using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.DTOs.Category
{
    public class CategoryAdminViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
    public class CategoryUserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

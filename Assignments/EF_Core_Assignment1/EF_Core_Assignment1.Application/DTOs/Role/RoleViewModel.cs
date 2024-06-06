using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.DTOs.Role
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; } // Role name from IdentityRole
        public string? Description { get; set; }
    }
}

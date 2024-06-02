using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }

    }
}

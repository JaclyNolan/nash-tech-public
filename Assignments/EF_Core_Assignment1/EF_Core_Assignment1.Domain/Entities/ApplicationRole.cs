using EF_Core_Assignment1.Persistance.Identity;
using Microsoft.AspNetCore.Identity;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
        // Nav Props
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}

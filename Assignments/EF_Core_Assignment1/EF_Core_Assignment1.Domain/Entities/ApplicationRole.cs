using Microsoft.AspNetCore.Identity;

namespace EF_Core_Assignment1.Domain.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }

    }
}

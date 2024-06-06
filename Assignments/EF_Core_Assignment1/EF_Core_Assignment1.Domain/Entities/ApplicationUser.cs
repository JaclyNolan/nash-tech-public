using EF_Core_Assignment1.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EF_Core_Assignment1.Persistance.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(256, ErrorMessage = "Username can't be more than 256 characters.")]
        public string? Name { get; set; }
        // Navigation Properties
        // Mentor said two of these navigation properties to the same table is not fine
        public ICollection<BookBorrowingRequest> RequestedBookBorrowingRequest { get; } = new List<BookBorrowingRequest>();
        public ICollection<BookBorrowingRequest> ActionedBookBorrowingRequest { get; } = new List<BookBorrowingRequest>();
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ICollection<ApplicationRole> Roles { get; set; }
    }
}

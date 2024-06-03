using EF_Core_Assignment1.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EF_Core_Assignment1.Persistance.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Navigation Properties
        // Mentor said two of these navigation properties to the same table is not fine
        public ICollection<BookBorrowingRequest> RequestedBookBorrowingRequest { get; } = new List<BookBorrowingRequest>();
        public ICollection<BookBorrowingRequest> ApprovedBookBorrowingRequest { get; } = new List<BookBorrowingRequest>(); 
    }
}

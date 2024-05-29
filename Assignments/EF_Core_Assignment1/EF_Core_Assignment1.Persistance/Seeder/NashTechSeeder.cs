using Bogus;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Contexts;
using System;
using System.Globalization;

namespace EF_Core_Assignment1.Persistance.Seeder
{
    public static class NashTechSeeder
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        public static void Seed(this NashTechContext _context)
        {
            if (_context.Books.Any())
            {
                Console.WriteLine("Database already seeded");
                // Data already seeded
                return;
            }

            // Seed Book
            var bookFaker = new Faker<Book>()
                .RuleFor(b => b.Name, f => f.Commerce.ProductName())
                .RuleFor(b => b.Description, f => f.Lorem.Paragraph());
            var books = bookFaker.Generate(10);
            _context.Books.AddRange(books);
            _context.SaveChanges();

            Console.WriteLine("Database seeded successfully");
        }
    }
}

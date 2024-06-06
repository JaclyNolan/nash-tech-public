using Bogus;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Configurations;
using EF_Core_Assignment1.Persistance.Contexts;
using EF_Core_Assignment1.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EF_Core_Assignment1.Persistance.Seeder
{
    public static class NashTechSeeder
    {
        public static async Task Seed(this NashTechContext context, IServiceProvider services)
        {
            context.SeedBook();
            await context.SeedRole(services);
            await context.SeedUser(services);
            context.SeedCategory();
            await context.SeedBookBorrowingRequestAndDetails(services);
        }

        public static void SeedBook(this NashTechContext context)
        {
            if (context.Books.Any())
            {
                Console.WriteLine("Book table already seeded");
                // Data already seeded
                return;
            }

            // Seed Book
            var bookFaker = new Faker<Book>()
                .RuleFor(b => b.Title, f => f.Commerce.ProductName())
                .RuleFor(b => b.Author, f => f.Person.FullName)
                .RuleFor(b => b.Description, f => f.Lorem.Paragraph());
            var books = bookFaker.Generate(10);
            context.Books.AddRange(books);
            context.SaveChanges();

            Console.WriteLine("Book table seeded successfully");
        }

        public static void SeedCategory(this NashTechContext context)
        {
            if (context.Categories.Any())
            {
                Console.WriteLine("Category table already seeded");
                return;
            }

            // Seed Categories
            var categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);
            var categories = categoryFaker.Generate(5);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Assign each book to a random category
            var random = new Random();
            var books = context.Books.ToList();
            foreach (var book in books)
            {
                var randomCategory = categories[random.Next(categories.Count)];
                book.CategoryId = randomCategory.Id;
            }
            context.Books.UpdateRange(books);
            context.SaveChanges();

            Console.WriteLine("Category table seeded successfully and books assigned to categories");
        }

        public static async Task SeedBookBorrowingRequestAndDetails(this NashTechContext context, IServiceProvider services)
        {
            if (context.BookBorrowingRequests.Any())
            {
                Console.WriteLine("BookBorrowingRequest table already seeded");
                return;
            }
            // Ensure user are seeded first
            await context.SeedUser(services);

            var random = new Random();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var users = userManager.Users.ToList();
            var books = context.Books.ToList();
            var borrowingSettings = ApplicationSettings.BorrowingSettings;

            // Seed Book Borrowing Requests
            var requests = new List<BookBorrowingRequest>();
            foreach (var user in users)
            {
                var requestFaker = new Faker<BookBorrowingRequest>()
                    .RuleFor(r => r.RequestorId, f => users[random.Next(users.Count)].Id)
                    .RuleFor(r => r.RequestDate, f => f.Date.Past(1))
                    .RuleFor(r => r.Status, f => BookRequestStatus.Waiting);

                requests.AddRange(requestFaker.Generate(random.Next(0, borrowingSettings.MaxRequestsPerMonth + 1)));
            }
            context.BookBorrowingRequests.AddRange(requests);
            await context.SaveChangesAsync();

            var requestDetails = new List<BookBorrowingRequestDetails>();
            var faker = new Faker();
            // Seed Book Borrowing Request Details
            foreach (var request in requests)
            {
                var shuffedBooks = books.OrderBy(b => random.Next()).ToList();
                for (int i = 0; i < borrowingSettings.MaxBooksPerRequest; i++)
                {
                    var requestDetail = new BookBorrowingRequestDetails
                    {
                        BookId = shuffedBooks[i].Id,
                        BorrowedDate = faker.Date.Past(1),
                        ReturnedDate = faker.Date.Past(1).AddDays(faker.Random.Int(1, 30)),
                        BookBorrowingRequestId = request.Id,
                    };
                    requestDetails.Add(requestDetail);
                }
            }
            context.BookBorrowingRequestDetails.AddRange(requestDetails);
            await context.SaveChangesAsync();

            Console.WriteLine("BookBorrowingRequest & BookBorrowingRequestDetail table seeded successfully");
        }

        public static async Task SeedRole(this NashTechContext _context, IServiceProvider serviceProvider)
        {
            if (_context.Roles.Any())
            {
                Console.WriteLine("Role table already seeded");
            }

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                var roleName = role.ToString();
                // Create roles using ApplicationRole instead of IdentityRole
                await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
            }
            Console.WriteLine("Role table seeded successfully");
        }

        public static async Task SeedUser(this NashTechContext context, IServiceProvider services)
        {
            if (context.Users.Any())
            {
                return;
            }
            // Ensure role are seeded first
            await context.SeedRole(services);

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed specific admin user
            var adminEmail = "anhbg330011@gmail.com";
            var seedPassword = "Password1!";
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Jacly",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, seedPassword);

            await userManager.AddToRoleAsync(adminUser, UserRole.Admin.ToString());

            // Seed specific normal user
            var userEmail = "anhnmbh00203@fpt.edu.vn";
            var normalUser = new ApplicationUser
            {
                UserName = userEmail,
                Email = userEmail,
                Name = "Jacly2",
                EmailConfirmed = true
            };
            var result1 = await userManager.CreateAsync(normalUser, seedPassword);
            Console.WriteLine($"What: {result1}");

            if (!result1.Succeeded)
            {
                Console.WriteLine(result1.Errors.Select(e => e.Description));
            }
            await userManager.AddToRoleAsync(normalUser, UserRole.User.ToString());

            // Seed 3 random admin users
            var adminFaker = new Faker<ApplicationUser>()
                .RuleFor(u => u.UserName, f => f.Internet.Email())
                .RuleFor(u => u.Email, (f, u) => u.UserName)
                .RuleFor(u => u.Name, f => f.Internet.UserName())
                .RuleFor(u => u.EmailConfirmed, true);

            for (int i = 0; i < 3; i++)
            {
                ApplicationUser user = adminFaker.Generate();
                var result = await userManager.CreateAsync(user, seedPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
                }
                else
                {
                    Console.WriteLine($"Failed to create admin user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            // Seed 10 regular users
            var userFaker = new Faker<ApplicationUser>()
                .RuleFor(u => u.UserName, f => f.Internet.Email())
                .RuleFor(u => u.Email, (f, u) => u.UserName)
                .RuleFor(u => u.Name, f => f.Internet.UserName())
                .RuleFor(u => u.EmailConfirmed, true);

            for (int i = 0; i < 10; i++)
            {
                var user = userFaker.Generate();
                var result = await userManager.CreateAsync(user, seedPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, UserRole.User.ToString());
                }
                else
                {
                    Console.WriteLine($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            Console.WriteLine("User table seeded successfully");
        }
    }
}

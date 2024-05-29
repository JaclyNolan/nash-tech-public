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
            if (_context.Departments.Any() || _context.Employees.Any() || _context.Projects.Any() || _context.Salaries.Any() || _context.Books.Any())
            {
                Console.WriteLine("Database already seeded");
                // Data already seeded
                return;
            }

            // Seed Departments
            List<Department> departments = new List<Department>
            {
                new Department { Name = "Software Development" },
                new Department { Name = "Finance" },
                new Department { Name = "Accountant" },
                new Department { Name = "HR" },
            };
            _context.Departments.AddRange(departments);
            _context.SaveChanges();

            // Seed Employees
            var employeeFaker = new Faker<Employee>()
                .RuleFor(e => e.Name, f => f.Person.FullName)
                .RuleFor(e => e.JoinedDate, f => f.Date.Between(DateTime.Parse("1/1/2020"), DateTime.Now))
                .RuleFor(e => e.DepartmentId, f => f.PickRandom(departments).Id);

            var employees = employeeFaker.Generate(20); // Generate 20 employees

            // Seed Salaries
            var salaryFaker = new Faker<Salary>()
                .RuleFor(s => s.SalaryAmount, f => (float)f.Finance.Amount(2000, 8000));

            foreach (var employee in employees)
            {
                employee.Salary = salaryFaker.Generate(); // Generate a salary for each employee
            }
            _context.Employees.AddRange(employees);
            _context.SaveChanges();

            // Seed Projects
            var projectFaker = new Faker<Project>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName());

            var projects = projectFaker.Generate(10); // Generate 10 projects
            _context.Projects.AddRange(projects);
            _context.SaveChanges();

            // Seed ProjectEmployees
            var projectEmployees = new List<ProjectEmployee>();
            var random = new Random();
            foreach (var employee in employees)
            {
                var numberOfProjects = random.Next(1, 4); // Random number between 1 and 2

                // Shuffle the projects list
                var shuffledProjects = projects.OrderBy(p => random.Next()).ToList();

                for (int i = 0; i < numberOfProjects; i++)
                {
                    var projectEmployee = new ProjectEmployee
                    {
                        ProjectId = shuffledProjects[i].Id,
                        EmployeeId = employee.Id,
                        Enable = new Faker().Random.Bool()
                    };

                    projectEmployees.Add(projectEmployee);
                }
            }

            _context.ProjectEmployees.AddRange(projectEmployees);
            _context.SaveChanges();

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

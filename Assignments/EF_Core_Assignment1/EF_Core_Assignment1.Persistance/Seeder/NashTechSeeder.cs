using Bogus;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Persistance.Seeder
{
    public static class NashTechSeeder
    {
        public static void SeederAsync(this NashTechContext _context)
        {
            if (_context.Departments.Any() || _context.Employees.Any() || _context.Projects.Any() || _context.Salaries.Any())
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
            var projectEmployeeFaker = new Faker<ProjectEmployee>()
                .RuleFor(pe => pe.ProjectId, f => f.PickRandom(projects).Id)
                .RuleFor(pe => pe.EmployeeId, f => f.PickRandom(employees).Id)
                .RuleFor(pe => pe.Enable, f => f.Random.Bool());

            var projectEmployees = projectEmployeeFaker.Generate(30); // Generate 30 project employees
            _context.ProjectEmployees.AddRange(projectEmployees);
            _context.SaveChanges();

            var salaries = salaryFaker.Generate(15); // Generate 15 salaries
            _context.Salaries.AddRange(salaries);
            _context.SaveChanges();
            Console.WriteLine("Database seeded successfully");
        }

        //private List<Department> GenerateDepartments()
        //{
            
        //    return departments;
        //}
        //private List<Employee> GenerateEmployees(int count)
        //{
        //    var faker = new Faker<Employee>()
        //        .RuleFor(e => e.Name, f => f.Person.FullName)
        //        .RuleFor(e => e.JoinedDate, f => f.Date.Between(DateTime.Parse("1/1/2020"), DateTime.Now))
        //        .RuleFor(e => e.)
        //}
    }


}

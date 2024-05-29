using EF_Core_Assignment1.Domain.Common;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EF_Core_Assignment1.Persistance.Contexts
{
    public class NashTechContext : IdentityDbContext<IdentityUser>
    {
        //public NashTechContext() : base()
        //{
        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=localhost;Database=nashtech;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True");
        //    base.OnConfiguring(optionsBuilder);
        //}
        public NashTechContext(DbContextOptions<NashTechContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure one-to-many relationship between Department and Employee
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .IsRequired();

            // Configure one-to-one relationship between Employee and Salary
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Salary)
                .WithOne(s => s.Employee)
                .HasForeignKey<Salary>(s => s.EmployeeId)
                .IsRequired();

            // Configure many-to-many relationship between Employee and Project
            modelBuilder.Entity<ProjectEmployee>()
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.ProjectId);

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.EmployeeId);

            // Configure many-to-many relationship between Project and Employee
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Projects)
                .WithMany(p => p.Employees)
                .UsingEntity<ProjectEmployee>(
                    j => j
                        .HasOne(pe => pe.Project)
                        .WithMany(p => p.ProjectEmployees)
                        .HasForeignKey(pe => pe.ProjectId),
                    j => j
                        .HasOne(pe => pe.Employee)
                        .WithMany(e => e.ProjectEmployees)
                        .HasForeignKey(pe => pe.EmployeeId)
                );

            // Configure other properties
            modelBuilder.Entity<Department>()
                .Property(d => d.Name)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .IsRequired();

            modelBuilder.Entity<Salary>()
                .Property(s => s.SalaryAmount)
                .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<ProjectEmployee>()
                .Property(pe => pe.Enable)
                .IsRequired();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimeStamp();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimeStamp()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var entity in modifiedEntities)
            {
                if (entity is BaseEntity baseEntity)
                {
                    if (baseEntity.DateCreated == null)
                    {
                        baseEntity.DateCreated = DateTime.UtcNow;
                    }
                    baseEntity.DateUpdated = DateTime.UtcNow;
                }
            }

        }
    }
}

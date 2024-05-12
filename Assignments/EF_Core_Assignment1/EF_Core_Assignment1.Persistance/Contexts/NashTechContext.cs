using EF_Core_Assignment1.Domain.Common;
using EF_Core_Assignment1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Persistance.Contexts
{
    public class NashTechContext : DbContext
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

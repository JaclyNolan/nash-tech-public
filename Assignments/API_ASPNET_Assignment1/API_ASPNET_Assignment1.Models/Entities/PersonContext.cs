using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_ASPNET_Assignment1.Models.Entities
{
    public class PersonContext : DbContext
    {
        public PersonContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("PeopleList");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimeStamp();
            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Person> People { get; set; } = null!;

        private void UpdateTimeStamp()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var entity in modifiedEntities)
            {
                if (entity is EntityBase baseEntity)
                {
                    if (baseEntity.CreatedAt == null)
                    {
                        baseEntity.CreatedAt = DateTime.UtcNow;
                    }
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

        }
    }
}

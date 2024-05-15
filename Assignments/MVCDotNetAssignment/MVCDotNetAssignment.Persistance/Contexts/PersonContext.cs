using Microsoft.EntityFrameworkCore;
using MVCDotNetAssignment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDotNetAssignment.Persistance.Contexts
{
    public class PersonContext : DbContext
    {
        public PersonContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("PeopleList");
        }

        public DbSet<Person> People { get; set; } = null!;
    }
}

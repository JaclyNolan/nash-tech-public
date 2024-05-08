using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_ASPNET_Assignment1.Models.Entities
{
    public class TaskContext : DbContext
    {
        public TaskContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TaskList");
        }
        public DbSet<TaskModel> Tasks { get; set; } = null!;
    }
}

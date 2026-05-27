using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Entities;
using ToDoList.Infrastructure.Configuration;

namespace ToDoList.Infrastructure.Context
{
    public class ToDoListDbContext : DbContext
    {
        public DbSet<UserTask> _tasks { get; set; }

        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TodoService.DAL.Data.Models;

namespace TodoService.DAL.Data
{
    public class TodoContext : DbContext
    {
        public DbSet<Todo> Todos => Set<Todo>();

        public TodoContext()
        {
            Database.EnsureCreated();
        }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            
            
            builder.Entity<Todo>(b =>
            {
                //b.HasKey(p => new { p.Id });
                //b.HasKey(p => new { p.UserId });
                //b.HasKey(p => new { p.UserId, p.Id });
                //modelBuilder.Entity<Foo>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            });

        }
    }
}

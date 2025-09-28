using Microsoft.EntityFrameworkCore;
using TodoListApi.Data.Models;

namespace TodoListApi.Data;

public class TodoApiContext : DbContext
{
    public TodoApiContext(DbContextOptions<TodoApiContext> options)
    : base(options)
    {
    }
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>(t =>
        {
            t.Property(t => t.Title).HasMaxLength(50);
        });
        base.OnModelCreating(modelBuilder);
    }
}

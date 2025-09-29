using Microsoft.EntityFrameworkCore;
using TodoListApi.Data.Models;

namespace TodoListApi.Data;

public class TodoApiContext : DbContext
{
    public TodoApiContext(
        DbContextOptions<TodoApiContext> options) : base(options)
    { 
        
    }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>(t =>
        {
            t.ToTable("Todos");
            t.HasKey(t => t.Id);
            t.Property(t => t.Title).HasMaxLength(1024);
            t.HasOne(t => t.User).WithMany(t => t.Todos).HasForeignKey(t => t.UserId);
        });
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(u =>
        {
            u.ToTable("users");
            u.HasKey(u => u.Id);
            u.Property(u => u.Name).HasMaxLength(126);
            u.Property(u => u.Token).HasMaxLength(16);
            u.HasMany(o => o.Todos).WithOne(o => o.User).HasForeignKey(o => o.UserId);
            u.HasIndex(t => t.Token).IsUnique();
        });
    }
}

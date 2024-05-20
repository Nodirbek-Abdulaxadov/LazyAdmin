using LazyAdmin.Models;
using Microsoft.EntityFrameworkCore;

namespace LazyAdmin.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
        Database.EnsureCreated();
    }

    public DbSet<Test> Tests { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}

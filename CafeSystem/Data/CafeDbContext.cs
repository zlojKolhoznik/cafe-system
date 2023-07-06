using CafeSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Data;

public class CafeDbContext : DbContext
{
    public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Category> Categories { get; set; }

    public DbSet<Dish> Dishes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // seeding categories as for Italian cafe (user is able to delete this any time they wish)
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Паста" },
            new Category { Id = 2, Name = "Піца" },
            new Category { Id = 3, Name = "Десерти" },
            new Category { Id = 4, Name = "Кава" },
            new Category { Id = 5, Name = "Чай" },
            new Category { Id = 6, Name = "Безалкогольні напої" },
            new Category { Id = 7, Name = "Алкогольні напої" }
        );
    }
}
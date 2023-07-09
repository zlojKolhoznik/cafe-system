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
    
    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }
}
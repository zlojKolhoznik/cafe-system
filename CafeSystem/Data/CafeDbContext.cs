using CafeSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CafeSystem.Data;

public class CafeDbContext : IdentityDbContext
{
    public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Employee>().HasData(
            new
            {
                Id = "2d2296cf-a816-4641-8327-901ca4553f4c",
                Discriminator = "Employee",
                Name = "Admin",
                Surname = "Admin",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@mycafe.com",
                NormalizedEmail = "ADMIN@MYCAFE.COM",
                EmailConfirmed = false,
                PasswordHash = "AQAAAAIAAYagAAAAENZWV+cIPXKzyNs8UdyqM13IlAGaEMEpnPxBvd2P+I4UJL9ttFYHe0RGvUKG8WYvTg==",
                SecurityStamp = "LZH2HEZRRY5CQQCEVVPII2EJ64ZELOSS",
                ConcurrencyStamp = "a3a4c124-e493-499a-9a50-b1d84d615e61",
                PhoneNumber = "0000000000",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0
            }
        );
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Dish> Dishes { get; set; }
    
    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }
    
    public DbSet<Employee> Employees { get; set; }
}
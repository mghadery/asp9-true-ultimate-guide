using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManager.Core.Domain.Entities;
using OrderManager.Infrastructure.Persistent.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Infrastructure.Persistent.DbContexts;

public class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {        
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Order
        modelBuilder.Entity<Order>().HasKey(x => x.OrderId);

        modelBuilder.Entity<Order>()
            .Property(x => x.OrderNumber)
            .HasMaxLength(30);

        modelBuilder.Entity<Order>()
            .Property(x => x.TotalAmount)
            .HasColumnType("Decimal(18,2)");

        modelBuilder.Entity<Order>()
            .Property(x => x.OrderDate)
            .HasColumnType("DateTime2(3)");

        //OrderItem
        modelBuilder.Entity<OrderItem>().HasKey(x => x.OrderItemId);
        
        modelBuilder.Entity<OrderItem>()
            .Property(x => x.UnitPrice)
            .HasColumnType("Decimal(18,2)");

        modelBuilder.Entity<OrderItem>()
            .Property(x => x.TotalPrice)
            .HasColumnType("Decimal(18,2)");

        modelBuilder.Entity<OrderItem>()
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId);

        base.OnModelCreating(modelBuilder);
    }
}

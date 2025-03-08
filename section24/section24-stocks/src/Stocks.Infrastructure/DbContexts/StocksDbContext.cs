using Microsoft.EntityFrameworkCore;
using Stocks.Core.Domain.Entities;

namespace Stocks.Infrastructure.DbContexts;

public class StocksDbContext : DbContext
{
    public StocksDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<BuyOrder> Orders { get; set; }
    public DbSet<SellOrder> SellOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BuyOrder>().ToTable(nameof(BuyOrder));
        modelBuilder.Entity<BuyOrder>().HasKey(buyOrder => buyOrder.BuyOrderID);
        modelBuilder.Entity<BuyOrder>().ToTable(x =>
        {
            x.HasCheckConstraint("priceCheck", "Price >= 1 and Price <= 10000");
            x.HasCheckConstraint("quantityCheck", "Quantity >= 1 and Quantity <= 100000");
        });

        modelBuilder.Entity<BuyOrder>()
            .Property(buyOrder => buyOrder.BuyOrderID)
            .HasColumnName(nameof(BuyOrder.BuyOrderID))
            .HasColumnType("uniqueidentifier");

        modelBuilder.Entity<BuyOrder>()
            .Property(buyOrder => buyOrder.StockName)
            .HasMaxLength(100);

        modelBuilder.Entity<BuyOrder>()
            .Property(buyOrder => buyOrder.StockSymbol)
            .HasMaxLength(100);

        modelBuilder.Entity<BuyOrder>()
            .Property(buyOrder => buyOrder.Quantity)
            .HasColumnType("int")
            .IsRequired();

        modelBuilder.Entity<BuyOrder>()
            .Property(buyOrder => buyOrder.Price)
            .IsRequired();

        modelBuilder.Entity<BuyOrder>()
            .Property(buyOrder => buyOrder.DateAndTimeOfOrder)
            .IsRequired()
            .HasColumnType("datetime2(3)");


        modelBuilder.Entity<SellOrder>().ToTable(nameof(SellOrder));
        modelBuilder.Entity<SellOrder>().HasKey(SellOrder => SellOrder.SellOrderID);
        modelBuilder.Entity<SellOrder>().ToTable(x =>
        {
            x.HasCheckConstraint("priceCheck", "Price >= 1 and Price <= 10000");
            x.HasCheckConstraint("quantityCheck", "Quantity >= 1 and Quantity <= 100000");
        });

        modelBuilder.Entity<SellOrder>()
            .Property(SellOrder => SellOrder.SellOrderID)
            .HasColumnName(nameof(SellOrder.SellOrderID))
            .HasColumnType("uniqueidentifier");

        modelBuilder.Entity<SellOrder>()
            .Property(SellOrder => SellOrder.StockName)
            .HasMaxLength(100);

        modelBuilder.Entity<SellOrder>()
            .Property(SellOrder => SellOrder.StockSymbol)
            .HasMaxLength(100);

        modelBuilder.Entity<SellOrder>()
            .Property(SellOrder => SellOrder.Quantity)
            .HasColumnType("int")
            .IsRequired();

        modelBuilder.Entity<SellOrder>()
            .Property(SellOrder => SellOrder.Price)
            .IsRequired();

        modelBuilder.Entity<SellOrder>()
            .Property(SellOrder => SellOrder.DateAndTimeOfOrder)
            .IsRequired()
            .HasColumnType("datetime2(3)");
    }
}

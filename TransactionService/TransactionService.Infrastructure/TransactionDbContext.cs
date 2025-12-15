using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure;

public class TransactionDbContext : DbContext
{
    public TransactionDbContext(DbContextOptions<TransactionDbContext> options)
        : base(options)
    {
    }

    public DbSet<StockTransaction> Transactions => Set<StockTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockTransaction>(builder =>
        {
            builder.ToTable("stock_transactions");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(t => t.ProductId).HasColumnName("product_id");
            builder.Property(t => t.Date).HasColumnName("date");
            builder.Property(t => t.Type).HasColumnName("type");
            builder.Property(t => t.Quantity).HasColumnName("quantity");
            builder.Property(t => t.UnitPrice).HasColumnName("unit_price");
            builder.Property(t => t.Detail).HasColumnName("detail").HasMaxLength(500);
            builder.HasIndex(t => new { t.ProductId, t.Date }).HasDatabaseName("ix_stock_transactions_product_date");
        });
    }
}

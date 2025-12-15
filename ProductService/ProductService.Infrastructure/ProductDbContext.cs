using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(builder =>
        {
            builder.ToTable("products");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            builder.Property(p => p.Description).HasColumnName("description").HasMaxLength(500);
            builder.Property(p => p.Category).HasColumnName("category").HasMaxLength(100);
            builder.Property(p => p.ImageUrl).HasColumnName("image_url").HasMaxLength(500);
            builder.Property(p => p.Price).HasColumnName("price");
            builder.Property(p => p.Stock).HasColumnName("stock");
            builder.HasIndex(p => p.Name).HasDatabaseName("ix_products_name");
        });
    }
}

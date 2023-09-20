using Microsoft.EntityFrameworkCore;
using Store.Models.Entities;

namespace Store.Data;

public sealed class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) {}

    public DbSet<Product> products { get; set; }
    public DbSet<SubCategory> sub_categories { get; set; } 
    public DbSet<Category> categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>()
            .HasOne(c => c.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(k => new { k.catId })
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Entity<Category>()
            .HasMany(s => s.Subcats)
            .WithOne(c => c.Category)
            .HasForeignKey(k => new { k.catId })
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(builder);
    }
}
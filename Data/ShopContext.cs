using Microsoft.EntityFrameworkCore;
using Coursework.Models;

namespace Coursework.Data;

public class ShopContext : DbContext
{
    public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }

    public DbSet<Category>    Categories   { get; set; } = null!;
    public DbSet<Chetkas>     Chetkas      { get; set; } = null!;
    public DbSet<CartItem>    CartItems    { get; set; } = null!;
    public DbSet<FavoriteItem> FavoriteItems { get; set; } = null!;
    public DbSet<ProductView> ProductViews { get; set; } = null!;
    public DbSet<Order>       Orders       { get; set; } = null!;
    public DbSet<OrderItem>   OrderItems   { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Chetkas
        modelBuilder.Entity<Chetkas>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Chetkas>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Chetkas)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Chetkas>().ToTable("Chetkas");

        // CartItem — уникальная пара (SessionId, ChetkasId)
        modelBuilder.Entity<CartItem>()
            .HasIndex(ci => new { ci.SessionId, ci.ChetkasId })
            .IsUnique();

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Chetkas)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.ChetkasId)
            .OnDelete(DeleteBehavior.Cascade);

        // FavoriteItem — уникальная пара (SessionId, ChetkasId)
        modelBuilder.Entity<FavoriteItem>()
            .HasIndex(fi => new { fi.SessionId, fi.ChetkasId })
            .IsUnique();

        modelBuilder.Entity<FavoriteItem>()
            .HasOne(fi => fi.Chetkas)
            .WithMany(c => c.FavoriteItems)
            .HasForeignKey(fi => fi.ChetkasId)
            .OnDelete(DeleteBehavior.Cascade);

        // ProductView
        modelBuilder.Entity<ProductView>()
            .HasIndex(pv => new { pv.SessionId, pv.ChetkasId });

        modelBuilder.Entity<ProductView>()
            .HasOne(pv => pv.Chetkas)
            .WithMany(c => c.ProductViews)
            .HasForeignKey(pv => pv.ChetkasId)
            .OnDelete(DeleteBehavior.Cascade);

        // Order → OrderItems
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Chetkas)
            .WithMany(c => c.OrderItems)
            .HasForeignKey(oi => oi.ChetkasId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

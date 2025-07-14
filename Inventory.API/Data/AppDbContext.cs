using Inventory.API.Models;
using Inventory.API.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = PasswordHasher.HashPassword("admin123"),
                    Email = "admin@inventory.com",
                    Role = "Admin",
                    IsDeleted = false
                }
            );
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(u => u.Username)
                    .IsUnique();

                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(u => u.Email)
                    .HasMaxLength(100);

                entity.Property(u => u.Role)
                    .HasMaxLength(50)
                    .HasDefaultValue("User");

                entity.Property(u => u.IsDeleted)
                    .HasDefaultValue(false);
            });
        }
        private void ConfigureProductModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Barcode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(p => p.Barcode)
                    .IsUnique();

                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.StockQty)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Category)
                    .HasMaxLength(50);

                entity.Property(p => p.Status)
                    .HasDefaultValue(true);

                entity.Property(p => p.IsDeleted)
                    .HasDefaultValue(false);
            });
        }

        private void ConfigureCustomerModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerId);
                entity.Property(c => c.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Phone)
                    .HasMaxLength(20);

                entity.Property(c => c.Email)
                    .HasMaxLength(100);

                entity.Property(c => c.LoyaltyPoints)
                    .HasDefaultValue(0);

                entity.Property(c => c.IsDeleted)
                    .HasDefaultValue(false);
            });
        }

        private void ConfigureSaleModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(s => s.SaleId);

                entity.Property(s => s.SaleDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(s => s.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(s => s.PaidAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(s => s.DueAmount)
                    .HasColumnType("decimal(18,2)");

            });

            modelBuilder.Entity<SaleDetail>(entity =>
            {
                entity.HasKey(sd => sd.SaleDetailId);

                entity.Property(sd => sd.Quantity)
                    .HasColumnType("decimal(18,2)");

                entity.Property(sd => sd.Price)
                    .HasColumnType("decimal(18,2)");

                // Relationship with Sale
              
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Handle soft delete and audit fields if needed
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified
                    || e.State == EntityState.Deleted));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Deleted)
                {
                    // Handle soft delete
                    entityEntry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
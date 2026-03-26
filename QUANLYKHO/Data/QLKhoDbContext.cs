using Microsoft.EntityFrameworkCore;
using QUANLYKHO.model;

namespace QUANLYKHO.Data
{
    public class QLKhoDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<ImportDetail> ImportDetails { get; set; }
        public DbSet<Export> Exports { get; set; }
        public DbSet<ExportDetail> ExportDetails { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<InventoryCheck> InventoryChecks { get; set; }
        public DbSet<InventoryCheckDetail> InventoryCheckDetails { get; set; }
        public DbSet<Factory> Factories { get; set; }
        public DbSet<UserFactory> UserFactories { get; set; }

        public QLKhoDbContext() { }
        public QLKhoDbContext(DbContextOptions<QLKhoDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.Instance.GetConnectionString(),
                    sqlOptions => sqlOptions.UseCompatibilityLevel(120));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Location)
                .WithMany(l => l.Products)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Location>()
                .HasOne(l => l.Shelf)
                .WithMany(s => s.Locations)
                .HasForeignKey(l => l.ShelfId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Factory)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.FactoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Import>()
                .HasOne(i => i.Factory)
                .WithMany()
                .HasForeignKey(i => i.FactoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Export>()
                .HasOne(e => e.Factory)
                .WithMany()
                .HasForeignKey(e => e.FactoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryCheck>()
                .HasOne(ic => ic.Factory)
                .WithMany()
                .HasForeignKey(ic => ic.FactoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.Factory)
                .WithMany(f => f.Warehouses)
                .HasForeignKey(w => w.FactoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFactory>()
                .HasKey(uf => new { uf.UserId, uf.FactoryId });

            modelBuilder.Entity<UserFactory>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFactories)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFactory>()
                .HasOne(uf => uf.Factory)
                .WithMany(f => f.UserFactories)
                .HasForeignKey(uf => uf.FactoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExportDetail>()
                .HasKey(ed => new { ed.ExportId, ed.ProductId });

            modelBuilder.Entity<ImportDetail>()
                .HasKey(id => id.Id);

            modelBuilder.Entity<InventoryCheckDetail>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<Product>()
                .Property(p => p.CurrentQuantity).HasPrecision(18, 3);
            modelBuilder.Entity<Product>()
                .Property(p => p.TotalImported).HasPrecision(18, 3);
            modelBuilder.Entity<Product>()
                .Property(p => p.TotalExported).HasPrecision(18, 3);
            modelBuilder.Entity<Product>()
                .Property(p => p.SafetyStock).HasPrecision(18, 3);

            modelBuilder.Entity<ImportDetail>()
                .Property(id => id.Quantity).HasPrecision(18, 3);

            modelBuilder.Entity<ExportDetail>()
                .Property(ed => ed.Quantity).HasPrecision(18, 3);

            modelBuilder.Entity<InventoryCheckDetail>()
                .HasOne(d => d.InventoryCheck)
                .WithMany(ic => ic.Details)
                .HasForeignKey(d => d.InventoryCheckId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryCheckDetail>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryCheck>()
                .HasOne(ic => ic.CreatedBy)
                .WithMany()
                .HasForeignKey(ic => ic.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryCheck>()
                .HasOne(ic => ic.ApprovedBy)
                .WithMany()
                .HasForeignKey(ic => ic.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ImportDetail>()
                .HasOne(d => d.Product)
                .WithMany(p => p.ImportDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExportDetail>()
                .HasOne(d => d.Product)
                .WithMany(p => p.ExportDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.LastImportBy)
                .WithMany()
                .HasForeignKey(p => p.LastImportById)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.FactoryId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.MaterialCode, p.FactoryId })
                .IsUnique()
                .HasDatabaseName("IX_Product_MaterialCode_FactoryId_Unique");

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Barcode)
                .IsUnique();

            modelBuilder.Entity<Warehouse>()
                .HasIndex(w => w.FactoryId);

            modelBuilder.Entity<UserFactory>()
                .HasIndex(uf => uf.UserId);

            modelBuilder.Entity<UserFactory>()
                .HasIndex(uf => uf.FactoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
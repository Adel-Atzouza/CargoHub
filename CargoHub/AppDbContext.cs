using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CargoHub
{
    public class AppDbContext : DbContext
    {
        // DbSet properties for all models
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ItemLine> ItemLines { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Client> Clients { get; set; }

        // Override SaveChangesAsync to handle timestamps
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseModel>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        // Configure model relationships and seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

    // Shipment - Order relationship: one shipment can have multiple orders
            modelBuilder.Entity<Shipment>()
                .HasMany(s => s.orders)          // One shipment has many orders
                .WithOne(o => o.Shipment)       // Each order has one shipment
                .HasForeignKey(o => o.ShipmentId)  // Foreign key in the Orders table
                .IsRequired(false);  // ShipmentId in Orders is optional (nullable)

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Shipment)
                .WithMany(s => s.orders)
                .HasForeignKey(o => o.ShipmentId)
                .IsRequired(false);  // Allow nulls for shipment in Orders (since some orders might not have shipments)

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShipToClient)
                .WithMany()
                .HasForeignKey(o => o.ShipTo);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillToClient)
                .WithMany()
                .HasForeignKey(o => o.BillTo);
                
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ItemId });  // Composite key for OrderItem

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(oi => oi.ItemId);  // Foreign key relationship using ItemId (int)

        }

        // Constructor accepting DbContextOptions
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
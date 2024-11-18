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
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ItemId });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(oi => oi.ItemId)
                .HasPrincipalKey(i => i.Uid); // Use Uid as the principal key

            base.OnModelCreating(modelBuilder);

            // Seed data for ItemGroup
            modelBuilder.Entity<ItemGroup>()
                .HasData(
                    new ItemGroup
                    {
                        Id = 1,
                        Name = "Electronics",
                        Description = "Items related to electronic devices and accessories.",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new ItemGroup
                    {
                        Id = 2,
                        Name = "Furniture",
                        Description = "Items for home and office furniture.",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new ItemGroup
                    {
                        Id = 3,
                        Name = "Stationery",
                        Description = "Items for writing, drawing, and office use.",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });

            // Seed data for ItemLine
            modelBuilder.Entity<ItemLine>()
                .HasData(
                    new ItemLine
                    {
                        Id = 1,
                        Name = "Laptop",
                        Description = "High-performance laptop for work and gaming.",
                        ItemGroupId = 3,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new ItemLine
                    {
                        Id = 2,
                        Name = "Office Chair",
                        Description = "Ergonomic chair for comfortable seating during long hours.",
                        ItemGroupId = 3,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new ItemLine
                    {
                        Id = 3,
                        Name = "Notebook",
                        Description = "Lined notebook for taking notes and organizing tasks.",
                        ItemGroupId = 3,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });

            base.OnModelCreating(modelBuilder);

                        modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    Id = 1,
                    Uid = "ITEM001",
                    Code = "ITEMCODE001",
                    Description = "High-quality widget",
                    ShortDescription = "Widget",
                    UpcCode = "123456789012",
                    ModelNumber = "WID001",
                    CommodityCode = "C001",
                    ItemLine = 1,
                    ItemGroup = 1,
                    ItemType = 1,
                    UnitPurchaseQuantity = 100,
                    UnitOrderQuantity = 1,
                    PackOrderQuantity = 10,
                    SupplierId = 1,
                    SupplierCode = "SUP001",
                    SupplierPartNumber = "SPN001",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Item
                {
                    Id = 2,
                    Uid = "ITEM002",
                    Code = "ITEMCODE002",
                    Description = "Durable gadget",
                    ShortDescription = "Gadget",
                    UpcCode = "987654321098",
                    ModelNumber = "GAD002",
                    CommodityCode = "C002",
                    ItemLine = 2,
                    ItemGroup = 2,
                    ItemType = 2,
                    UnitPurchaseQuantity = 200,
                    UnitOrderQuantity = 2,
                    PackOrderQuantity = 20,
                    SupplierId = 2,
                    SupplierCode = "SUP002",
                    SupplierPartNumber = "SPN002",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Call the base method
            base.OnModelCreating(modelBuilder);
        }

        // Constructor accepting DbContextOptions
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

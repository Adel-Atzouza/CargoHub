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

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>()
            .HasOne(l => l.warehouse)  // A Location has one Warehouse
            .WithMany()                 // A Warehouse can have many Locations
            .HasForeignKey(l => l.WarehouseId) ; // Location uses WarehouseId as a foreign key
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


                // Seed data for Orders
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    SourceId = 1,
                    OrderDate = DateTime.UtcNow,
                    RequestDate = DateTime.UtcNow.AddDays(1),
                    Reference = "ORD123",
                    ExtrReference = "Urgent Delivery",
                    OrderStatus = "Pending",
                    Notes = "Order notes",
                    ShippingNotes = "Handle with care",
                    PickingNotes = "Fragile items",
                    WarehouseId = 1,
                    ShipTo = 1,
                    BillTo = 1,
                    ShipmentId = null,
                    TotalAmount = 250.00M,
                    TotalDiscount = 10.00M,
                    TotalTax = 5.00M,
                    TotalSurcharge = 2.00M,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed data for OrderItems
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    OrderId = 1,
                    ItemId = 1, // Referring to Item 1
                    Amount = 3
                },
                new OrderItem
                {
                    OrderId = 1,
                    ItemId = 2, // Referring to Item 2
                    Amount = 2
                }
            );

            base.OnModelCreating(modelBuilder);
        }

        // Constructor accepting DbContextOptions
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
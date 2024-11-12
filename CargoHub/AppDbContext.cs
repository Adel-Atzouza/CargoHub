using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;



namespace CargoHub
{
    public class AppDbContext : DbContext
    {
<<<<<<< HEAD
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ItemLine> ItemLines { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }

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


        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<ItemGroup>()
            //     .HasOne(Igr => Igr.ItemLines)
            //     .WithMany();

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
=======
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship between Order and Item
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ItemId }); // Composite key

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(oi => oi.ItemId);

            // Seed data for Orders
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    SourceId = 33,
                    OrderDate = new DateTime(2019, 4, 3, 11, 33, 15),
                    RequestDate = new DateTime(2019, 4, 7, 11, 33, 15),
                    Reference = "ORD00001",
                    ExtraReference = "Bedreven arm straffen bureau.",
                    OrderStatus = "Delivered",
                    Notes = "Voedsel vijf vork heel.",
                    ShippingNotes = "Buurman betalen plaats bewolkt.",
                    PickingNotes = "Ademen fijn volgorde scherp aardappel op leren.",
                    WarehouseId = 18,
                    ShipmentId = 1,
                    TotalAmount = 9905.13m,
                    TotalDiscount = 150.77m,
                    TotalTax = 372.72m,
                    TotalSurcharge = 77.6m,
                    CreatedAt = new DateTime(2019, 4, 3, 11, 33, 15),
                    UpdatedAt = new DateTime(2019, 4, 5, 7, 33, 15)
                }
            );

            // Seed data for Items
            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Uid = "P007435", Code = "Item1", Description = "First item", UnitOrderQuantity = 23 },
                new Item { Id = 2, Uid = "P009557", Code = "Item2", Description = "Second item", UnitOrderQuantity = 1 },
                new Item { Id = 3, Uid = "P009553", Code = "Item3", Description = "Third item", UnitOrderQuantity = 50 }
            );

            // Seed data for OrderItems (join table)
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { OrderId = 1, ItemId = 1, Amount = 23 },
                new OrderItem { OrderId = 1, ItemId = 2, Amount = 1 },
                new OrderItem { OrderId = 1, ItemId = 3, Amount = 50 }
            );
>>>>>>> origin/sven3
        }
    }
}

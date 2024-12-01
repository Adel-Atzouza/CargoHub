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

            modelBuilder.Entity<Shipment>()
                .HasMany(s => s.orders)          // Een zending kan meerdere orders bevatten
                .WithOne(o => o.Shipment)       // Elke order hoort bij één zending
                .HasForeignKey(o => o.ShipmentId)  // Dit is de koppeling via ShipmentId in de Orders-tabel
                .IsRequired(false);  // ShipmentId mag leeg zijn, want niet elke order heeft een zending

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Shipment)       // Een order kan gekoppeld zijn aan één zending
                .WithMany(s => s.orders)       // Een zending kan meerdere orders bevatten
                .HasForeignKey(o => o.ShipmentId) // De koppeling is via ShipmentId in de Orders-tabel
                .IsRequired(false);            // Toegestaan dat een order geen zending heeft (nullable)

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShipToClient)   // Een order heeft één ontvanger (ShipTo)
                .WithMany()                    // Er is geen navigatie terug naar alle orders van de ontvanger
                .HasForeignKey(o => o.ShipTo); // De koppeling gebeurt via ShipTo (foreign key)

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillToClient)   // Een order heeft één klant die de rekening krijgt (BillTo)
                .WithMany()                    // Geen navigatie terug naar alle orders van de klant
                .HasForeignKey(o => o.BillTo); // De koppeling gebeurt via BillTo (foreign key)

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ItemUid });  // Primaire sleutel: combinatie van OrderId en ItemUid

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)        // Een OrderItem hoort bij één order
                .WithMany(o => o.OrderItems)   // Een order kan meerdere OrderItems hebben
                .HasForeignKey(oi => oi.OrderId); // De koppeling is via OrderId

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)         // Een OrderItem hoort bij één specifiek item
                .WithMany(i => i.OrderItems)   // Een item kan in meerdere orders voorkomen
                .HasForeignKey(oi => oi.ItemUid);  // De koppeling is via ItemUid


        }

        // Constructor accepting DbContextOptions
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}}
using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Warehouse> Warehouses {get; set;}
        public DbSet<Contact> Contacts {get; set;}
        public DbSet<Order> Orders {get;set;}
        public DbSet<Shipment> Shipments {get; set;}
        public DbSet<Location> Location {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relationship
            modelBuilder.Entity<Warehouse>()
                .HasOne(r => r.Contact)
                .WithOne()
                .HasForeignKey<Warehouse>(c => c.ContactId);
        }
    }
}
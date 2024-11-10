using CargoHub.Controllers;
using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Item_type> Item_Types { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Contact> Contacts { get; set; }

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
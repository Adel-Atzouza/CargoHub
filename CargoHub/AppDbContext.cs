using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Warehouse> Warehouses {get; set;}
        public DbSet<Contact> Contacts {get; set;}
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relationship
            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.Contact)
                .WithOne()
                .HasForeignKey<Warehouse>(c => c.ContactId);
        }
    }
}
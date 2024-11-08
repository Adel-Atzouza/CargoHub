using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Warehouse> Warehouses {get; set;}
        public DbSet<Contact> Contacts {get; set;}
        public DbSet<ItemLine> ItemLines {get; set;}
        public DbSet<ItemGroup> ItemGroups {get; set;}
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relationship
            modelBuilder.Entity<Warehouse>()
                .HasOne(r => r.Contact)
                .WithOne()
                .HasForeignKey<Warehouse>(c => c.ContactId);
            modelBuilder.Entity<ItemGroup>()
                        .HasData(
        new ItemGroup
        {
            Id = 1,
            Name = "Electronics",
            Description = "Items related to electronic devices and accessories."
        },
        new ItemGroup
        {
            Id = 2,
            Name = "Furniture",
            Description = "Items for home and office furniture."
        },
        new ItemGroup
        {
            Id = 3,
            Name = "Stationery",
            Description = "Items for writing, drawing, and office use."
        }); 
        }
    }
}
using CargoHub.Controllers;
using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        // public DbSet<Contact> Contacts { get; set; }
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
        }
    }
}

using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemGroup>()
                .HasMany(g => g.ItemLines)
                .WithOne(l => l.ItemGroup)
                .HasForeignKey(l => l.ItemGroupId);

            // ItemLine -> ItemTypes (One-to-Many)
            modelBuilder.Entity<ItemLine>()
                .HasMany(l => l.ItemTypes)
                .WithOne(t => t.ItemLine)
                .HasForeignKey(t => t.ItemLineId);

            // ItemType -> Items (One-to-Many)
            modelBuilder.Entity<ItemType>()
                .HasMany(t => t.Items)
                .WithOne(i => i.ItemType)
                .HasForeignKey(i => i.ItemTypeId);


        }
    }
}

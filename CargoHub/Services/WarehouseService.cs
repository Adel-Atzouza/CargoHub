using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub
{
    public class WarehouseService
    {
        private readonly AppDbContext appDbContext;
        public WarehouseService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Warehouse?> GetWarehouse(int id)
        {
            return await appDbContext.Warehouses
                // .Include(w => w.Contact) // Include the Contact related to the Warehouse
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<Warehouse>> GetAllWarehouses()
        {
            return await appDbContext.Warehouses
                // .Include(w => w.Contact) // Include the Contact related to the Warehouse
                .ToListAsync();
        }

        public async Task<int?> PostWarehouse(Warehouse Warehouse)
        {
            var warehouse = Warehouse with { Id = appDbContext.Warehouses.Any() ? appDbContext.Warehouses.Max(w => w.Id) + 1 : 1, CreatedAt = DateTime.Now };
            await appDbContext.Warehouses.AddAsync(warehouse);
            int n = await appDbContext.SaveChangesAsync();
            return n > 0 ? warehouse.Id : null;
        }

        public async Task<int?> PutWarehouse(int id, Warehouse Warehouse)
        {
            var warehouse = await appDbContext.Warehouses.FindAsync(id);
            if (warehouse == null) return null;

            appDbContext.Entry(warehouse).CurrentValues.SetValues(Warehouse);
            warehouse.UpdatedAt = DateTime.Now;

            int n = await appDbContext.SaveChangesAsync();
            return n > 0 ? warehouse.Id : null;
        }

        public async Task<bool> DeleteWarehouse(int id)
        {
            var warehouse = await appDbContext.Warehouses.FindAsync(id);
            if (warehouse == null) return false;
            appDbContext.Warehouses.Remove(warehouse);
            int n = await appDbContext.SaveChangesAsync();
            return n > 0;
        }
    }
}
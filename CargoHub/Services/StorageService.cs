using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub
{
    public class StorageService
    {
        private readonly AppDbContext appDbContext;
        public StorageService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Warehouse?> GetWarehouse(int id)
        {
            return await appDbContext.Warehouses
                .Include(w => w.Contact) // Include the Contact related to the Warehouse
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<Warehouse>> GetAllWarehouses()
        {
            return await appDbContext.Warehouses
                .Include(w => w.Contact) // Include the Contact related to the Warehouse
                .ToListAsync();
        }

        public async Task<int?> PostWarehouse(Warehouse warehouse)
        {
            var war = warehouse with { Id = appDbContext.Warehouses.Any() ? appDbContext.Warehouses.Max(w => w.Id) + 1 : 1, CreatedAt = DateTime.Now };
            await appDbContext.Warehouses.AddAsync(war);
            int n = await appDbContext.SaveChangesAsync();
            return n > 0 ? war.Id : null;
        }

        public async Task<int?> PutWarehouse(int id, Warehouse warehouse)
        {
            var war = await appDbContext.Warehouses.FindAsync(id);
            if (war == null) return null;
            war.Name = warehouse.Name;
            war.Code = warehouse.Code;
            war.Address = warehouse.Address;
            war.Contact = warehouse.Contact;
            war.UpdatedAt = DateTime.Now;
            appDbContext.Warehouses.Update(war);
            int n = await appDbContext.SaveChangesAsync();
            return n > 0 ? war.Id : null;
        }

        public async Task<bool> DeleteWarehouse(int id)
        {
            var war = await appDbContext.Warehouses.FindAsync(id);
            if (war == null) return false;
            appDbContext.Warehouses.Remove(war);
            int n = await appDbContext.SaveChangesAsync();
            return n > 0;
        }
    }
}
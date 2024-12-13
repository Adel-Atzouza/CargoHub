using Microsoft.EntityFrameworkCore;
using CargoHub.Models;

namespace CargoHub.Services
{
    public class SupplierService
    {
        private readonly AppDbContext appDbContext;
        public SupplierService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Supplier?> GetSupplier(int id)
        {
            return await appDbContext.Suppliers
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<Supplier>> GetAllSuppliers()
        {
            return await appDbContext.Suppliers.ToListAsync();
        }

        // public async Task<int?> PostSupplier(Supplier Supplier)
        // {
        //     var supplier = Supplier with { Id = appDbContext.Suppliers.Any() ? appDbContext.Suppliers.Max(w => w.Id) + 1 : 1 };
        //     await appDbContext.Suppliers.AddAsync(supplier);
        //     int n = await appDbContext.SaveChangesAsync();
        //     return n > 0 ? supplier.Id : null;
        // }

        public async Task<int?> PutSupplier(int id, Supplier Supplier)
        {
            var supplier = await appDbContext.Suppliers.FindAsync(id);
            if (supplier == null) return null;

            appDbContext.Entry(supplier).CurrentValues.SetValues(Supplier);
            supplier.UpdatedAt = DateTime.Now;

            int n = await appDbContext.SaveChangesAsync();
            return n > 0 ? supplier.Id : null;
        }

        public async Task<bool> DeleteSupplier(int id)
        {
            var supplier = await appDbContext.Suppliers.FindAsync(id);
            if (supplier == null) return false;
            appDbContext.Suppliers.Remove(supplier);
            int n = await appDbContext.SaveChangesAsync();
            return n > 0;
        }
    }
}
using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub.Services
{
    public class LocationStorageService(AppDbContext appDbContext) : BaseStorageService(appDbContext)
    {
        private new readonly AppDbContext appDbContext = appDbContext;

        public async Task<List<Location>> GetLocationsInWarehouse(int warehouseId)
        {
            return await appDbContext.Locations
                .Where(location => location.WarehouseId == warehouseId)
                .ToListAsync();
        }
    }
}
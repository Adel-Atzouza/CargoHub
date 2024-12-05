using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class LocationStorageService(AppDbContext appDbContext) : BaseStorageService(appDbContext)
    {
        public async Task<List<Location>> GetLocationsInWarehouse(int warehouseId)
        {
            return await appDbContext.Locations
                .Where(location => location.WarehouseId == warehouseId)
                .ToListAsync();
        }
    }
}
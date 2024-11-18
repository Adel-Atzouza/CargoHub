using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class LocationService
    {
        private readonly AppDbContext _context;

        public LocationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetLocations(int id)
        {
            return await _context.Locations // Corrected to 'Locations'
                .Where(location => location.Id >= id)
                .OrderBy(location => location.Id)
                .Take(100)
                .ToListAsync();
        }

        public async Task<Location> GetLocation(int id)
        {
            return await _context.Locations // Corrected to 'Locations'
                .FirstOrDefaultAsync(location => location.Id == id);
        }

        public async Task<List<Location>> GetLocationWarehouse(int id)
        {
            return await _context.Locations // Corrected to 'Locations'
                .Where(loc => loc.WarehouseId == id)
                .ToListAsync();
        }

       public async Task<string> AddLocation(Location location)
        {
            try
            {
                location.CreatedAt = DateTime.UtcNow;
                location.UpdatedAt = DateTime.UtcNow;

                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
                return "Location added successfully.";
            }
            catch (Exception ex)
            {
                return $"Error adding location: {ex.Message}";
            }
        }

        public async Task<string> UpdateLocation(int locationId, Location updatedLocation)
        {
            var existingLocation = await _context.Locations.FindAsync(locationId); // Corrected to 'Locations'
            if (existingLocation == null)
            {
                return "Error: Location not found.";
            }

            updatedLocation.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingLocation).CurrentValues.SetValues(updatedLocation);
            await _context.SaveChangesAsync();
            return "Location updated successfully.";
        }

        public async Task<string> RemoveLocation(int locationId)
        {
            var location = await _context.Locations.FindAsync(locationId); // Corrected to 'Locations'
            if (location == null)
            {
                return "Error: Location not found.";
            }

            _context.Locations.Remove(location); // Corrected to 'Locations'
            await _context.SaveChangesAsync();
            return "Location removed successfully.";
        }
    }
}

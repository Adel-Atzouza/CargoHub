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

        // Constructor: Inject de databasecontext (AppDbContext)
        public LocationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetAllLocations()
        {
            return await _context.Locations
                .ToListAsync();
        }
        // Haal één specifieke locatie op op basis van ID
        public async Task<Location> GetLocation(int id)
        {
            return await _context.Locations
                .FirstOrDefaultAsync(location => location.Id == id); // Zoek naar de eerste locatie met de juiste ID
        }

        // Haal alle locaties op die horen bij een specifieke warehouse
        public async Task<List<Location>> GetLocationWarehouse(int id)
        {
            return await _context.Locations
                .Where(loc => loc.WarehouseId == id) // Filter: WarehouseId moet overeenkomen
                .ToListAsync(); // Haal de lijst asynchroon op
        }

        // Voeg een nieuwe locatie toe
        public async Task<string> AddLocation(Location location)
        {
            // Check of WarehouseId geldig is
            if (location.WarehouseId != null && !await _context.Warehouses.AnyAsync(w => w.Id == location.WarehouseId))
            {
                return "Ongeldige WarehouseId opgegeven."; // Foutmelding als de WarehouseId niet bestaat
            }

            // Stel de aanmaak- en update-tijd in
            location.CreatedAt = DateTime.UtcNow;
            location.UpdatedAt = DateTime.UtcNow;

            // Voeg de locatie toe aan de database
            _context.Locations.Add(location);
            await _context.SaveChangesAsync(); // Sla de wijzigingen op
            return "Locatie succesvol toegevoegd."; // Succesbericht
        }

        // Werk een bestaande locatie bij
        public async Task<string> UpdateLocation(int locationId, Location updatedLocation)
        {
            // Zoek de bestaande locatie op
            var existingLocation = await _context.Locations.FindAsync(locationId);
            if (existingLocation == null)
            {
                return "Fout: Locatie niet gevonden."; // Foutmelding als de locatie niet bestaat
            }

            // Update alleen de velden die mogen worden aangepast
            existingLocation.WarehouseId = updatedLocation.WarehouseId;
            existingLocation.Code = updatedLocation.Code;
            existingLocation.Name = updatedLocation.Name;
            existingLocation.UpdatedAt = DateTime.UtcNow; // Update de bewerktijd

            // Sla de wijzigingen op
            await _context.SaveChangesAsync();
            return "Locatie succesvol bijgewerkt."; // Succesbericht
        }

        // Verwijder een locatie op basis van ID
        public async Task<bool> RemoveLocation(int locationId)
        {
            // Zoek de locatie op
            var location = await _context.Locations.FindAsync(locationId);
            if (location == null)
            {
                return false;
            }

            // Verwijder de locatie uit de database
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync(); // Sla de wijzigingen op
            return true ; // Succesbericht
        }
    }
}

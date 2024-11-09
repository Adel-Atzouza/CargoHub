using CargoHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CargoHub.Services
{
    public class LocationService{
        private readonly AppDbContext _context;

        public LocationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetLocations(int id)
        {
            return await _context.Location
            .Where(location => location.Id >= id)
            .OrderBy(location => location.Id)
            .Take(100)
            .ToListAsync();
        }

        public async Task<Location> GetLocation(int id)
        {
            return await _context.Location.FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<List<Location>> GetLocationWareHouse(int id)
        {
            return await _context.Location
            .Where(loc => loc.WarehouseId == id)
            .ToListAsync();
        }

        public async Task<string> AddLocation(Location location)
        {
            location.CreatedAt = DateTime.UtcNow;
            location.UpdatedAt = DateTime.UtcNow;
            _context.Location.Add(location);
            await _context.SaveChangesAsync();
            return "Location added successfully.";
        }

        public async Task<string> UpdateLocation(int locationId, Location updatedLocation)
        {
            var existingLocation = await _context.Location.FindAsync(locationId);
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
            var location = await _context.Location.FindAsync(locationId);
            if (location == null)
            {
                return "Error: Location not found.";
            }

            _context.Location.Remove(location);
            await _context.SaveChangesAsync();
            return "Location removed successfully.";
        }
    }
}















// class Locations(Base):
//     def __init__(self, root_path, is_debug=False):
//         self.data_path = root_path + "locations.json"
//         self.load(is_debug)

//     def get_locations(self):
//         return self.data

//     def get_location(self, location_id):
//         for x in self.data:
//             if x["id"] == location_id:
//                 return x
//         return None

//     def get_locations_in_warehouse(self, warehouse_id):
//         result = []
//         for x in self.data:
//             if x["warehouse_id"] == warehouse_id:
//                 result.append(x)
//         return result

//     def add_location(self, location):
//         location["created_at"] = self.get_timestamp()
//         location["updated_at"] = self.get_timestamp()
//         self.data.append(location)

//     def update_location(self, location_id, location):
//         location["updated_at"] = self.get_timestamp()
//         for i in range(len(self.data)):
//             if self.data[i]["id"] == location_id:
//                 self.data[i] = location
//                 break

//     def remove_location(self, location_id):
//         for x in self.data:
//             if x["id"] == location_id:
//                 self.data.remove(x)

//     def load(self, is_debug):
//         if is_debug:
//             self.data = LOCATIONS
//         else:
//             f = open(self.data_path, "r")
//             self.data = json.load(f)
//             f.close()

//     def save(self):
//         f = open(self.data_path, "w")
//         json.dump(self.data, f)
//         f.close()

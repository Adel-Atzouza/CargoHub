using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class InventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        // Get a list of inventories, with an optional filter based on a minimum ID (like in LocationService)
        public async Task<List<Inventory>> GetInventories(int id = 0)
        {
            return await _context.Inventory
                .Where(inventory => inventory.Id >= id)
                .OrderBy(inventory => inventory.Id)
                .Take(100)
                .ToListAsync();
        }

        // Get a single inventory by ID
        public async Task<Inventory> GetInventory(int id)
        {
            return await _context.Inventory.FirstOrDefaultAsync(inventory => inventory.Id == id);
        }

        // Add a new inventory
        public async Task<string> AddInventory(Inventory inventory)
        {
            inventory.CreatedAt = DateTime.UtcNow;
            inventory.UpdatedAt = DateTime.UtcNow;
            _context.Inventory.Add(inventory);
            await _context.SaveChangesAsync();
            return "Inventory added successfully.";
        }

        // Update an existing inventory by ID
        public async Task<string> UpdateInventory(int id, Inventory updatedInventory)
        {
            var existingInventory = await _context.Inventory.FindAsync(id);
            if (existingInventory == null)
            {
                return "Error: Inventory not found.";
            }

            updatedInventory.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingInventory).CurrentValues.SetValues(updatedInventory);
            await _context.SaveChangesAsync();
            return "Inventory updated successfully.";
        }

        // Delete an inventory by ID
        public async Task<string> DeleteInventory(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
            {
                return "Error: Inventory not found.";
            }

            _context.Inventory.Remove(inventory);
            await _context.SaveChangesAsync();
            return "Inventory deleted successfully.";
        }
    }
}

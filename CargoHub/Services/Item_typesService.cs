
using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class ItemTypesService
    {
        private readonly AppDbContext _context;

        public ItemTypesService(AppDbContext context)
        {
            _context = context;
        }

        // Get up to 100 Item_type records, ordered by Id
        public async Task<List<Item_type>> GetItemTypes()
        {
            return await _context.Item_Types
                .OrderBy(itemType => itemType.Id)
                .Take(100) // Limit to 100 records
                .ToListAsync();
        }

        // Get a single Item_type by Id
        public async Task<Item_type> GetItemTypeById(int id)
        {
            return await _context.Item_Types.FirstOrDefaultAsync(itemType => itemType.Id == id);
        }

        // Create a new Item_type
        public async Task<Item_type> PostItemType(Item_type newItemType)
        {
            newItemType.CreatedAt = DateTime.UtcNow;
            newItemType.UpdatedAt = DateTime.UtcNow;

            _context.Item_Types.Add(newItemType);
            await _context.SaveChangesAsync();

            return newItemType;
        }

        // Delete an Item_type by Id
        public async Task<bool> DeleteItemTypeById(int id)
        {
            var itemType = await _context.Item_Types.FindAsync(id);
            if (itemType == null)
            {
                return false;
            }

            _context.Item_Types.Remove(itemType);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update an Item_type by Id
        public async Task<string> UpdateItemType(int id, Item_type updatedItemType)
        {
            var existingItemType = await _context.Item_Types.FindAsync(id);
            if (existingItemType == null)
            {
                return "Error: Item type not found.";
            }

            updatedItemType.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingItemType).CurrentValues.SetValues(updatedItemType);
            await _context.SaveChangesAsync();

            return "Item type updated.";
        }
    }
}


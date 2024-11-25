
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
        public async Task<List<ItemType>> GetItemTypes()
        {
            return await _context.ItemTypes
                .OrderBy(itemType => itemType.Id)
                .Take(100) // Limit to 100 records
                .ToListAsync();
        }

        // Get a single Item_type by Id
        public async Task<ItemType> GetItemTypeById(int id)
        {
            return await _context.ItemTypes.FirstOrDefaultAsync(itemType => itemType.Id == id);
        }

        // Create a new Item_type
        public async Task<ItemType> PostItemType(ItemType newItemType)
        {
            _context.ItemTypes.Add(newItemType);
            await _context.SaveChangesAsync();

            return newItemType;
        }

        // Delete an Item_type by Id
        public async Task<bool> DeleteItemTypeById(int id)
        {
            var itemType = await _context.ItemTypes.FindAsync(id);
            if (itemType == null)
            {
                return false;
            }

            _context.ItemTypes.Remove(itemType);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update an Item_type by Id
        public async Task<string> UpdateItemType(int id, ItemType updatedItemType)
        {
            var existingItemType = await _context.ItemTypes.FindAsync(id);
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


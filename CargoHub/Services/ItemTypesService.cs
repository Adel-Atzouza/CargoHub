
using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class ItemTypesService
    {
        private readonly AppDbContext appDbContext;

        public ItemTypesService(AppDbContext context)
        {
            appDbContext = context;
        }

        // Get up to 100 Item_type records, ordered by Id
        public async Task<List<ItemType>> GetItemTypes()
        {
            return await appDbContext.ItemTypes
                .OrderBy(itemType => itemType.Id)
                .Take(100) // Limit to 100 records
                .ToListAsync();
        }

        // Get a single Item_type by Id
        public async Task<ItemType> GetItemTypeById(int id)
        {
            return await appDbContext.ItemTypes.FirstOrDefaultAsync(itemType => itemType.Id == id);
        }

        // Create a new Item_type
        public async Task<ItemType> PostItemType(ItemType newItemType)
        {
            appDbContext.ItemTypes.Add(newItemType);
            await appDbContext.SaveChangesAsync();

            return newItemType;
        }

        // Delete an Item_type by Id
        public async Task<bool> DeleteItemTypeById(int id)
        {
            var itemType = await appDbContext.ItemTypes.FirstOrDefaultAsync(_ => _.Id == id);
            if (itemType == null)
            {
                return false;
            }

            appDbContext.ItemTypes.Remove(itemType);
            await appDbContext.SaveChangesAsync();
            return true;
        }

        // Update an Item_type by Id
        public async Task<string> UpdateItemType(int id, ItemType updatedItemType)
        {
            var existingItemType = await appDbContext.ItemTypes.FirstOrDefaultAsync(_ => _.Id == id);
            if (existingItemType == null)
            {
                return "Error: Item type not found.";
            }

            updatedItemType.UpdatedAt = DateTime.UtcNow;
            appDbContext.Entry(existingItemType).CurrentValues.SetValues(updatedItemType);
            await appDbContext.SaveChangesAsync();

            return "Item type updated.";
        }
    }
}


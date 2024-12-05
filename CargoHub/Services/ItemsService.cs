using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class ItemsService
    {
        private readonly AppDbContext appDbContext;

        public ItemsService(AppDbContext context)
        {
            appDbContext = context;
        }

        public async Task<List<Item>> GetAllItems()
        {
            return await appDbContext.Items.ToListAsync();
        }

        public async Task<Item> GetItem(string uid)
        {
            return await appDbContext.Items.FirstOrDefaultAsync(item => item.Uid == uid);
        }


        public async Task<Item> PostItems(Item newItem)
        {
            // Add the new item to the DbContext
            appDbContext.Items.Add(newItem);

            // Save changes to the database (persist the item)
            await appDbContext.SaveChangesAsync();

            // Return the created item (you can also modify this to return only the ID or other fields if needed)
            return newItem;
        }

        public async Task<bool> DeleteItemsByUid(string uid)
        {
            var item = await appDbContext.Items.FirstOrDefaultAsync(_ => _.Uid == uid);
            if (item == null)
            {
                return false;
            }

            appDbContext.Items.Remove(item);
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<string> UpdateItem(string uid, Item updatedItem)
        {
            // Find the existing item by UID
            var existingItem = await appDbContext.Items.FirstOrDefaultAsync(_ => _.Uid == uid);
            if (existingItem == null)
            {
                return "Error: Item not found.";
            }

            // Update only the modifiable fields, excluding the primary key (Id) and CreatedAt
            existingItem.Uid = updatedItem.Uid;
            existingItem.Code = updatedItem.Code;
            existingItem.Description = updatedItem.Description;
            existingItem.ShortDescription = updatedItem.ShortDescription;
            existingItem.UpcCode = updatedItem.UpcCode;
            existingItem.ModelNumber = updatedItem.ModelNumber;
            existingItem.CommodityCode = updatedItem.CommodityCode;
            existingItem.ItemLine = updatedItem.ItemLine;
            existingItem.ItemGroup = updatedItem.ItemGroup;
            existingItem.ItemType = updatedItem.ItemType;
            existingItem.UnitPurchaseQuantity = updatedItem.UnitPurchaseQuantity;
            existingItem.UnitOrderQuantity = updatedItem.UnitOrderQuantity;
            existingItem.PackOrderQuantity = updatedItem.PackOrderQuantity;
            existingItem.SupplierId = updatedItem.SupplierId;
            existingItem.SupplierCode = updatedItem.SupplierCode;
            existingItem.SupplierPartNumber = updatedItem.SupplierPartNumber;

            // Update the 'UpdatedAt' timestamp
            existingItem.UpdatedAt = DateTime.UtcNow;

            // Save the changes
            await appDbContext.SaveChangesAsync();
            return "Item updated.";
        }
    }
}
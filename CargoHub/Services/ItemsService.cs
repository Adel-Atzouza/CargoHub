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
        private readonly AppDbContext _context;

        public ItemsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemDto>> GetAllItems()
        {
            var items = await _context.Items.ToListAsync();
            return items.Select(MapToDto).ToList();
        }

        public async Task<ItemDto?> GetItem(string uid)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == uid);
            return item == null ? null : MapToDto(item);
        }

        public async Task<Item> PostItems(Item newItem)
        {
            // Validate the foreign key references
            bool isValid = await ValidateItemType(newItem.ItemType, newItem.ItemGroup, newItem.ItemLine);
            if (!isValid)
            {
                throw new InvalidOperationException("Invalid ItemType, ItemGroup, or ItemLine provided.");
            }

            // Check for duplicate UID
            var existingItem = await GetItem(newItem.Uid);
            if (existingItem != null)
            {
                throw new InvalidOperationException("An item with the same UID already exists.");
            }

            // Add the new item to the database
            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();

            return newItem;
        }


        public async Task<bool> DeleteItemsByUid(string uid)
        {
            var item = await _context.Items.FirstOrDefaultAsync(_ => _.Uid == uid);
            if (item == null)
            {
                return false;
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> UpdateItem(string uid, Item updatedItem)
        {
            // Find the existing item by UID
            var existingItem = await _context.Items.FirstOrDefaultAsync(_ => _.Uid == uid);
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
            await _context.SaveChangesAsync();
            return "Item updated.";
        }

        public async Task<bool> ValidateItemType(int itemTypeId, int itemGroupId, int itemLineId)
        {
            bool isItemTypeValid = await _context.ItemTypes.AnyAsync(it => it.Id == itemTypeId);
            bool isItemGroupValid = await _context.ItemGroups.AnyAsync(ig => ig.Id == itemGroupId);
            bool isItemLineValid = await _context.ItemLines.AnyAsync(il => il.Id == itemLineId);

            // Return true only if all validations pass
            return isItemTypeValid && isItemGroupValid && isItemLineValid;
        }


        private ItemDto MapToDto(Item item)
        {
            return new ItemDto
            {
                Uid = item.Uid,
                Code = item.Code,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                UpcCode = item.UpcCode,
                ModelNumber = item.ModelNumber,
                CommodityCode = item.CommodityCode,
                ItemLine = item.ItemLine,
                ItemGroup = item.ItemGroup,
                ItemType = item.ItemType,
                UnitPurchaseQuantity = item.UnitPurchaseQuantity,
                UnitOrderQuantity = item.UnitOrderQuantity,
                PackOrderQuantity = item.PackOrderQuantity,
                SupplierId = item.SupplierId,
                SupplierCode = item.SupplierCode,
                SupplierPartNumber = item.SupplierPartNumber
            };
        }
    }
}
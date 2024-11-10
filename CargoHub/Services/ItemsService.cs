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

        public async Task<List<Item>> GetItems(string uid)
        {
            return await _context.Items
                .Where(item => item.Uid.CompareTo(uid) >= 0)  // Filter items die gelijk of groter zijn dan de opgegeven 'uid'
                .OrderBy(item => item.Uid)                    // Sorteer op 'Uid'
                .Take(100)                                    // Beperk het aantal records tot 100
                .ToListAsync();                               // Voer de query uit en haal de lijst op
        }

        public async Task<Item> GetItem(string uid)
        {
            return await _context.Items.FirstOrDefaultAsync(item => item.Uid == uid);
        }


        public async Task<Item> PostItems(Item newItem)
        {
            // Add the new item to the DbContext
            _context.Items.Add(newItem);

            // Save changes to the database (persist the item)
            await _context.SaveChangesAsync();

            // Return the created item (you can also modify this to return only the ID or other fields if needed)
            return newItem;
        }

        public async Task<bool> DeleteItemsByUid(string uid)
        {
            var item = await _context.Items.FindAsync(uid);
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
            var existingItem = await _context.Items.FindAsync(uid);
            if (existingItem == null)
            {
                return "Error: Item not found.";
            }

            // Update the 'UpdatedAt' timestamp
            updatedItem.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingItem).CurrentValues.SetValues(updatedItem);
            await _context.SaveChangesAsync();
            return "Item updated.";
        }
    }
}
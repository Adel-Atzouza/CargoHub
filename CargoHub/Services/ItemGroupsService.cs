using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
namespace CargoHub.Services
{


    public class ItemGroupsService
    {
        private AppDbContext _context;
        public ItemGroupsService(AppDbContext context)
        {
            _context = context;
        }
        // public IEnumerable<ItemGroup> GetAllItemGroups(int page)
        // {
        //     var ItemGroups = _context.ItemGroups.OrderBy(g => g.Id);
        //     return ItemGroups;
        // }
        public async Task<List<object>> GetMultipleItemGroups(int[] GroupsIds)
        {
            List<object> FoundItemGroups = new();
            foreach (int id in GroupsIds)
            {
                ItemGroup? FoundItemGroup = await GetItemGroup(id);
                if (FoundItemGroup == null)
                {
                    FoundItemGroups.Add($"Item group with id: {id} was not found");
                }
                else
                {
                    FoundItemGroups.Add(FoundItemGroup);
                }
            }
            return FoundItemGroups;
        }

        public async Task<ItemGroup?> GetItemGroup(int Id)
        {
            ItemGroup? ItemGroup = await _context.ItemGroups.FindAsync(Id);
            Console.WriteLine("========");
            Console.WriteLine(ItemGroup);
            Console.WriteLine("========");
            return ItemGroup;
        }

    public async Task<bool> AddItemGroup(ItemGroup itemGroup)
    {
        bool AlreadyExists = await _context.ItemGroups.ContainsAsync(itemGroup);
        if (itemGroup == null || AlreadyExists)
        {
            return false;
        }
        _context.ItemGroups.Add(itemGroup);
        int result = await _context.SaveChangesAsync();
        return result > 0 ? true : false;
    }



        public async Task<bool> UpdateItemGroup(int id, ItemGroup itemGroup)
        {
            ItemGroup? Found = await _context.ItemGroups.FindAsync(id);
            // if the entity doesn't exist or the item group is not valid, return false
            if (itemGroup == null || Found == null) return false;
            Found.Name = itemGroup.Name;
            Found.Description = itemGroup.Description;
            _context.ItemGroups.Update(Found);
            int result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> DeleteItemGroup(int id)
        {
            ItemGroup? Found = await _context.ItemGroups.FindAsync(id);
            if (Found == null) return false;
            _context.ItemGroups.Remove(Found);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;

        }

    }
}
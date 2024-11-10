using CargoHub;
using Microsoft.EntityFrameworkCore;

public class ItemGroupsService
{
    private AppDbContext _context;
    public ItemGroupsService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<object>> GetMultipleItemGroups(int[] GroupsIds)
    {
        List<object> FoundItemGroups = new();
        foreach(int id in GroupsIds)
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
        await _context.SaveChangesAsync();
        return true;



    }

    public async Task<bool> UpdateItemGroup(int id, ItemGroup itemGroup)
    {
        ItemGroup? Found = await _context.ItemGroups.FindAsync(id);
        // if the entity doesn't exist or the item group is not valid, return false
        if (itemGroup == null || Found == null) return false;
        Found.Name = itemGroup.Name;
        Found.Description = itemGroup.Description;
        _context.ItemGroups.Update(Found);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteItemGroup(int id)
    {
        ItemGroup? Found = await _context.ItemGroups.FindAsync(id);
        if (Found == null) return false;
        _context.ItemGroups.Remove(Found);
        _context.SaveChanges();
        return true;

    }

}
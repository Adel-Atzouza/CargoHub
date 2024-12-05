using CargoHub;
using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

public class ItemGroupsService
{
    private AppDbContext appDbContext;
    public ItemGroupsService(AppDbContext context)
    {
        appDbContext = context;
    }

    public async Task<ItemGroup?> GetItemGroup(int id)
    {
        ItemGroup? ItemGroup = await appDbContext.ItemGroups.FirstOrDefaultAsync(_ => _.Id == id);
        return ItemGroup;
    }
    
        public async Task<Warehouse?> GetWarehouse(int id)
        {
            return await appDbContext.Warehouses
                .FirstOrDefaultAsync(w => w.Id == id);
        }

    public async Task<bool> AddItemGroup(ItemGroup itemGroup)
    {
        bool AlreadyExists = await appDbContext.ItemGroups.ContainsAsync(itemGroup);
        if (itemGroup == null || AlreadyExists)
        {
            return false;
        }
        appDbContext.ItemGroups.Add(itemGroup);
        await appDbContext.SaveChangesAsync();
        return true;



    }

    public async Task<bool> UpdateItemGroup(int id, ItemGroup itemGroup)
    {
        ItemGroup? Found = await appDbContext.ItemGroups.FirstOrDefaultAsync(_ => _.Id == id);
        // if the entity doesn't exist or the item group is not valid, return false
        if (itemGroup == null || Found == null) return false;
        Found.Name = itemGroup.Name;
        Found.Description = itemGroup.Description;
        appDbContext.ItemGroups.Update(Found);
        await appDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteItemGroup(int id)
    {
        ItemGroup? Found = await appDbContext.ItemGroups.FirstOrDefaultAsync(_ => _.Id == id);
        if (Found == null) return false;
        appDbContext.ItemGroups.Remove(Found);
        appDbContext.SaveChanges();
        return true;

    }

}
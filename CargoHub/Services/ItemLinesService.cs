using CargoHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ItemLinesService
{
    private AppDbContext appDbContext;
    public ItemLinesService(AppDbContext context)
    {
        appDbContext = context;
    }
    public async Task<List<object>> GetMultipleItemLines(int[] LinesIds)
    {
        List<object> FoundItemLines = new();
        foreach(int id in LinesIds)
        {
            ItemLine? FoundItemLine = await GetItemLine(id);
            if (FoundItemLine == null)
            {
                FoundItemLines.Add($"Item Line with id: {id} was not found");
            }
            else
            {
                FoundItemLines.Add(FoundItemLine);
            }
        }
        return FoundItemLines;
    }
    public async Task<ItemLine?> GetItemLine(int Id)
    {
        ItemLine? ItemLine = await appDbContext.ItemLines.FirstOrDefaultAsync(_ => _.Id == id);
        return ItemLine;
    }

    public async Task<bool> AddItemLine(ItemLine itemLine)
    {
        bool AlreadyExists = await appDbContext.ItemLines.ContainsAsync(itemLine);
        if (itemLine == null || AlreadyExists)
        {
            return false;
        }
        appDbContext.ItemLines.Add(itemLine);
        appDbContext.SaveChanges();
        return true;

    }

    public async Task<bool> UpdateItemLine(int id, ItemLine itemLine)
    {
        ItemLine? Found = await appDbContext.ItemLines.FirstOrDefaultAsync(_ => _.Id == id);
        if(itemLine == null || Found == null) return false;
        Found.Name = itemLine.Name;
        Found.Description = itemLine.Description;
        appDbContext.ItemLines.Update(Found);
        appDbContext.SaveChanges();
        return true;
    }

    public async Task<bool> DeleteItemLine(int id)
    {
        ItemLine? Found = await appDbContext.ItemLines.FirstOrDefaultAsync(_ => _.Id == id);
        if(Found == null) return false;
        appDbContext.ItemLines.Remove(Found);
        appDbContext.SaveChanges();
        return true;

    }

}
using CargoHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ItemLinesService
{
    private AppDbContext _context;
    public ItemLinesService(AppDbContext context)
    {
        _context = context;
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
        ItemLine? ItemLine = await _context.ItemLines.FindAsync(Id);
        return ItemLine;
    }

    public async Task<bool> AddItemLine(ItemLine itemLine)
    {
        bool AlreadyExists = await _context.ItemLines.ContainsAsync(itemLine);
        if (itemLine == null || AlreadyExists)
        {
            return false;
        }
        _context.ItemLines.Add(itemLine);
        _context.SaveChanges();
        return true;

    }

    public async Task<bool> UpdateItemLine(int id, ItemLine itemLine)
    {
        ItemLine? Found = await _context.ItemLines.FindAsync(id);
        if(itemLine == null || Found == null) return false;
        Found.Name = itemLine.Name;
        Found.Description = itemLine.Description;
        _context.ItemLines.Update(Found);
        _context.SaveChanges();
        return true;
    }

    public async Task<bool> DeleteItemLine(int id)
    {
        ItemLine? Found = await _context.ItemLines.FindAsync(id);
        if(Found == null) return false;
        _context.ItemLines.Remove(Found);
        _context.SaveChanges();
        return true;

    }

}
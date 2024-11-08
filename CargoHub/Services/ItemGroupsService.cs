using CargoHub;

public class ItemGroupsService
{
    private AppDbContext _context;
    public ItemGroupsService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ItemGroup> GetItemGroup(int Id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddItemGroup(ItemGroup itemGroup)
    {
        throw new NotImplementedException();

    }

    public async Task<bool> UpdateItemGroup(int id, ItemGroup itemGroup)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteItemGroup(int id)
    {
        throw new NotImplementedException();

    }

}
using CargoHub.Models;
using Microsoft.AspNetCore.Http.Features;
namespace CargoHub.Test;

// note: in-memory database is used because it's faster than an actual database
// TestSetup() : is used to setup the in-memory database before each test so the tests don't effect each other
// TestCleanup() : is used to delete the in-memory database so  

[TestClass]
public class TestItemGroupsService
{
    private AppDbContext _context;
    private ItemGroupsService _IgService;
    [TestInitialize]
    public void SetUpTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _context = new AppDbContext(options);

        _IgService = new ItemGroupsService(_context);
    }
    [TestMethod]
    public async Task TestPostItemGroup()
    {
        ItemGroup ig = new ItemGroup
        {
            Name = "Test",
            Description = "Test"
        };
        bool result = await _IgService.AddItemGroup(ig);
        Assert.IsTrue(result);
        ItemGroup? FoundIg = await _context.ItemGroups.FindAsync(1);
        Assert.IsNotNull(FoundIg);
        Assert.IsNotNull(FoundIg.CreatedAt);
        Assert.IsNotNull(FoundIg.UpdatedAt);
        Assert.AreEqual(ig.Name, FoundIg.Name);
    }
    [TestMethod]
    public async Task TestInvalidInput()
    {
        ItemGroup InvalidItemGroup = new ItemGroup
        {
            Name = null,
            Description = null
        };
        bool result = await _IgService.AddItemGroup(InvalidItemGroup);
        Assert.IsFalse(result);
        Assert.IsTrue(_context.ItemGroups.ToList().Count == 0);
        


    }
    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
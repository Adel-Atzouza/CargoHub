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
    public void Setup()
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
        ItemGroup Ig = TestHelper.TestItemGroup1;
        bool result = await _IgService.AddItemGroup(Ig);
        Assert.IsTrue(result);
        ItemGroup? FoundIg = await _context.ItemGroups.FindAsync(1);
        Assert.IsNotNull(FoundIg);
        Assert.IsNotNull(FoundIg.CreatedAt);
        Assert.IsNotNull(FoundIg.UpdatedAt);
        Assert.AreEqual(Ig.Name, FoundIg.Name);
    }
    [TestMethod]
    public async Task TesGetItemGroup()
    {
        ItemGroup Ig = TestHelper.TestItemGroup1;
        await _context.ItemGroups.AddAsync(Ig);
        await _context.SaveChangesAsync();
        ItemGroup? result = await _IgService.GetItemGroup(1);
        // Check if the properties are added correctly
        Assert.IsNotNull(result);
        Assert.AreEqual(Ig.Name, result.Name);
        Assert.AreEqual(Ig.Description, result.Description);


        // Check if the CreatedAt and UpdatedAt properties are set correctly
        TimeSpan tolerance = TimeSpan.FromMinutes(1);

        Assert.IsTrue(TestHelper.HaveSameDates(DateTime.Now, result.CreatedAt, tolerance));

    }

    [TestMethod]
    public async Task TestUpdateItemGroup()
    {
        ItemGroup Ig = TestHelper.TestItemGroup1;
        await _context.ItemGroups.AddAsync(Ig);
        await _context.SaveChangesAsync();
        ItemGroup UpdatedIg = TestHelper.TestItemGroup2;
        bool result = await _IgService.UpdateItemGroup(Ig.Id, UpdatedIg);
        Assert.IsTrue(result);
        Assert.IsTrue(Ig.UpdatedAt > Ig.CreatedAt);
    }

    [TestMethod]
    public async Task TestUpdateInvalidItemGroup()
    {
        // try to update an ItemGroup that doesn't exist
        ItemGroup UpdatedIg = TestHelper.TestItemGroup2;
        bool Result1 = await _IgService.UpdateItemGroup(999, UpdatedIg);
        Assert.IsFalse(Result1);

        // Add an ItemGroup to the DB
        ItemGroup Ig = TestHelper.TestItemGroup1;
        await _context.ItemGroups.AddAsync(Ig);
        await _context.SaveChangesAsync();

        // Test Updating the item group
        bool result = await _IgService.UpdateItemGroup(Ig.Id, UpdatedIg);
        Assert.IsTrue(result);

    }

    [TestMethod]
    public async Task TestDeleteItemGroup()
    {
        // try deleting an entity that doesn't exist
        bool Result1 = await _IgService.DeleteItemGroup(1);
        Assert.IsFalse(Result1);
        ItemGroup Ig = TestHelper.TestItemGroup1;
        await _context.ItemGroups.AddAsync(Ig);
        await _context.SaveChangesAsync();
        bool Result2 = await _IgService.DeleteItemGroup(1);
        Assert.IsTrue(Result2);
    }
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
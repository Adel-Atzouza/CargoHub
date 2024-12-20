
using Microsoft.AspNetCore.Http.Features;
namespace CargoHub.Test;

// note: in-memory database is used because it's faster than an actual database
// TestSetup() : is used to setup the in-memory database before each test so the tests don't effect each other
// TestCleanup() : is used to delete the in-memory database so  

[TestClass]
public class TestItemLinesService
{
    private AppDbContext _context;
    private ItemLinesService _IgService;
    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _context = new AppDbContext(options);

        _IgService = new ItemLinesService(_context);
    }
    [TestMethod]
    public async Task TestPostItemLine()
    {
        ItemLine Ig = TestHelper.TestItemLine1;
        bool result = await _IgService.AddItemLine(Ig);
        Assert.IsTrue(result);
        ItemLine? FoundIg = await _context.ItemLines.FindAsync(1);
        Assert.IsNotNull(FoundIg);
        Assert.IsNotNull(FoundIg.CreatedAt);
        Assert.IsNotNull(FoundIg.UpdatedAt);
        Assert.AreEqual(Ig.Name, FoundIg.Name);
    }
    [TestMethod]
    public async Task TesGetItemLine()
    {
        ItemLine Ig = TestHelper.TestItemLine1;
        await _context.ItemLines.AddAsync(Ig);
        await _context.SaveChangesAsync();
        ItemLine? result = await _IgService.GetItemLine(1);
        // Check if the properties are added correctly
        Assert.IsNotNull(result);
        Assert.AreEqual(Ig.Name, result.Name);
        Assert.AreEqual(Ig.Description, result.Description);


        // Check if the CreatedAt and UpdatedAt properties are set correctly
        TimeSpan tolerance = TimeSpan.FromMinutes(1);

        Assert.IsTrue(TestHelper.HaveSameDates(DateTime.Now, result.CreatedAt, tolerance));

    }

    [TestMethod]
    public async Task TestUpdateItemLine()
    {
        ItemLine Ig = TestHelper.TestItemLine1;
        await _context.ItemLines.AddAsync(Ig);
        await _context.SaveChangesAsync();
        ItemLine UpdatedIg = TestHelper.TestItemLine2;
        bool result = await _IgService.UpdateItemLine(Ig.Id, UpdatedIg);
        Assert.IsTrue(result);
        Assert.IsTrue(Ig.UpdatedAt > Ig.CreatedAt);
    }

    [TestMethod]
    public async Task TestUpdateInvalidItemLine()
    {
        // try to update an ItemLine that doesn't exist
        ItemLine UpdatedIg = TestHelper.TestItemLine2;
        bool Result1 = await _IgService.UpdateItemLine(999, UpdatedIg);
        Assert.IsFalse(Result1);

        // Add an ItemLine to the DB
        ItemLine Ig = TestHelper.TestItemLine1;
        await _context.ItemLines.AddAsync(Ig);
        await _context.SaveChangesAsync();

        // Test Updating the item group
        bool result = await _IgService.UpdateItemLine(Ig.Id, UpdatedIg);
        Assert.IsTrue(result);

    }

    [TestMethod]
    public async Task TestDeleteItemLine()
    {
        // try deleting an entity that doesn't exist
        bool Result1 = await _IgService.DeleteItemLine(1);
        Assert.IsFalse(Result1);
        ItemLine Ig = TestHelper.TestItemLine1;
        await _context.ItemLines.AddAsync(Ig);
        await _context.SaveChangesAsync();
        bool Result2 = await _IgService.DeleteItemLine(1);
        Assert.IsTrue(Result2);
    }
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
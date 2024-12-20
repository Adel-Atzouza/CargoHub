using CargoHub.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace CargoHub.Test;
[TestClass]
public class TestItemGroupsController
{
    private ItemGroupsController _controller;
    private ItemGroupsService _service;
    private AppDbContext _context;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;

        _context = new AppDbContext(options);
        _service = new(_context);
        _controller = new(_service);
    }

    [TestMethod]
    public async Task TestGetItemGroup()
    {
        // Get an ItemGroup that doesn't exist
        IActionResult NotFound_Response = await _controller.GetItemGroup(999);
        Assert.IsInstanceOfType(NotFound_Response, typeof(NotFoundObjectResult));

        // add an ItemGroup and try to Get it
        ItemGroup Ig = TestHelper.TestItemGroup1;
        await _context.ItemGroups.AddAsync(Ig);
        await _context.SaveChangesAsync();

        IActionResult Ok_Response = await _controller.GetItemGroup(Ig.Id);
        Assert.IsInstanceOfType(Ok_Response, typeof(OkObjectResult));

    }



    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }


}

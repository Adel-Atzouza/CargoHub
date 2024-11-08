using CargoHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoHub.Controllers
{
    [Route("api/warehouse")]
    public class WarehouseController(AppDbContext appDbContext) : Controller
    {
        AppDbContext appDbContext = appDbContext;
        [HttpGet()]
        public async Task<IActionResult> GetWarehouse([FromQuery] int id)
        {
            // return Ok(id);
            return Ok(await appDbContext.Warehouses
                .Include(w => w.Contact) // Include the Contact related to the Warehouse
                .FirstOrDefaultAsync(w => w.Id == id));
        }

        [HttpPost()]
        public async Task<IActionResult> PostWarehouse([FromBody] Warehouse warehouse)
        {
            var war = warehouse with {Id=appDbContext.Warehouses.Count() != 0 ? appDbContext.Warehouses.Max(w => w.Id) + 1 : 1, CreatedAt=DateTime.Now};
            await appDbContext.Warehouses.AddAsync(war);
            int n = await appDbContext.SaveChangesAsync();
            return Ok(n > 0 ? war.Id : false);
        }

    }
}
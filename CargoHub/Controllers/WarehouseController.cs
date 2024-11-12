using CargoHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoHub.Controllers
{
    [Route("api/v1/warehouses")]
    public class WarehouseController(AppDbContext appDbContext, WarehouseService storage) : Controller
    {
        AppDbContext appDbContext = appDbContext;
        WarehouseService storage = storage;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouse(int id)
        {               
            var warehouse = await storage.GetWarehouse((int)id);
            
            if (warehouse == null)
                return NotFound("Warehouse doesn't exist with id: " + id);
            
            return Ok(warehouse);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllWarehouses([FromQuery] int page = 0)
        {
            var warehouses = await storage.GetAllWarehouses();
            var paginatedWarehouses = warehouses.Skip(page * 100).Take(100).ToList();
            return Ok(paginatedWarehouses);
        }

        [HttpPost()]
        public async Task<IActionResult> PostWarehouse([FromBody] Warehouse warehouse)
        {
            if (warehouse == null)
            return BadRequest("Warehouse cannot be null");

            var createdWarehouseId = await storage.PostWarehouse(warehouse);
            return CreatedAtAction(nameof(GetWarehouse), new { id = createdWarehouseId }, createdWarehouseId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse(int id, [FromBody] Warehouse warehouse)
        {
            if (warehouse == null)
                return BadRequest("Warehouse cannot be null");

            var updatedWarehouse = await storage.PutWarehouse(id, warehouse);
            if (updatedWarehouse == null)
                return NotFound("Warehouse doesn't exist with id: " + id);

            return Ok(updatedWarehouse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            var warehouse = await storage.GetWarehouse(id);
            if (warehouse == null)
                return NotFound("Warehouse doesn't exist with id: " + id);

            return Ok(await storage.DeleteWarehouse(id));
        }
    }
}
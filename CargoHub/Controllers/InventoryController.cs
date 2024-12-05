using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Controllers
{
    [Route("api/v2/inventories")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: api/Inventory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventoryById(int id)
        {
            var inventory = await _inventoryService.GetInventory(id);
            if (inventory == null)
                return NotFound("Inventory not found.");

            return Ok(inventory);
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<List<Inventory>>> GetInventories([FromQuery] int id = 0)
        {
            var inventories = await _inventoryService.GetInventories(id);
            return Ok(inventories);
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<ActionResult<string>> CreateInventory([FromBody] Inventory inventory)
        {
            var result = await _inventoryService.AddInventory(inventory);
            return Ok(result); // Assuming `result` is a success message or status
        }

        // PUT: api/Inventory/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateInventory(int id, [FromBody] Inventory inventory)
        {
            var result = await _inventoryService.UpdateInventory(id, inventory);
            if (result == "Error: Inventory not found.")
                return NotFound(result);

            return Ok(result);
        }

        // DELETE: api/Inventory/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteInventory(int id)
        {
            var result = await _inventoryService.DeleteInventory(id);
            if (result == "Error: Inventory not found.")
                return NotFound(result);

            return Ok(result);
        }
    }
}

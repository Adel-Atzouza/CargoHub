using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]
    public class LocationsController(LocationStorageService storage) : BaseController<Location>(storage)
    {
        [HttpGet("warehouse/{warehouseId}")]
        public async Task<IActionResult> GetLocationsInWarehouse(int warehouseId)
        {
            var warehouse = await storage.GetRow<Warehouse>(warehouseId);
            if (warehouse == null)
                return NotFound($"Warehouse with ID {warehouseId} not found.");

            var locations = await storage.GetLocationsInWarehouse(warehouseId);
            return Ok(locations);
        }
    }
}

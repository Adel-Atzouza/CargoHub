using CargoHub.Models;
using CargoHub.Services;
using CargoHub.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]
    public class LocationsController(LocationStorageService storage, ErrorHandler error) : BaseController<Location>(storage, error)
    {
        [HttpGet("warehouse/{warehouseId}")]
        public async Task<IActionResult> GetLocationsInWarehouse(int warehouseId)
        {
            var warehouse = await storage.GetRow<Warehouse>(warehouseId);
            if (warehouse == null)
                return error.IdNotFound("Warehouse", warehouseId.ToString());

            var locations = await storage.GetLocationsInWarehouse(warehouseId);
            return Ok(locations);
        }
    }
}

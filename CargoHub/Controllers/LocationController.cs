using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocationById(int id)
        {
            var location = await _locationService.GetLocation(id);
            if (location == null) return NotFound("Location not found.");
            return Ok(location);
        }

        [HttpGet]
        public async Task<ActionResult<List<Location>>> GetLocations([FromQuery] int id)
        {
            return await _locationService.GetLocations(id);
        }

        [HttpGet("warehouse/{warehouseId}")]
        public async Task<ActionResult<List<Location>>> GetLocationsInWarehouse(int warehouseId)
        {
            return await _locationService.GetLocationWarehouse(warehouseId);
        }
            [HttpPost]
            public async Task<ActionResult<string>> AddLocation([FromBody] Location location)
            {
                var result = await _locationService.AddLocation(location);
                if (result.Contains("Invalid"))
                {
                    return BadRequest(result); // Return a bad request if there's an invalid WarehouseId
                }
                return Ok(result);
            }


        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateLocation(int id, [FromBody] Location location)
        {
            var result = await _locationService.UpdateLocation(id, location);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> RemoveLocation(int id)
        {
            var result = await _locationService.RemoveLocation(id);
            return Ok(result);
        }
    }
}

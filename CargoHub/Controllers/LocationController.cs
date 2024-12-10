using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Controllers
{
    [Route("api/v1/Locations")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;

        // Constructor: Inject de service die alle locatie-logica regelt
        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        // GET: api/v1/location/{id}
        // Haal een specifieke locatie op met een ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocationById(int id)
        {
            var location = await _locationService.GetLocation(id);
            if (location == null)
            {
                return NotFound("Locatie niet gevonden."); // 404 als de locatie niet bestaat
            }
            return Ok(location); // 200 met de gevraagde locatie
        }

        // GET: api/v1/location
        // Haal een lijst van locaties op, eventueel gefilterd op ID
        [HttpGet]
        public async Task<ActionResult<List<Location>>> GetLocations([FromQuery] int id, int page = 0)
        {
            var locations = await _locationService.GetAllLocations();
            var paginatedLocations = locations.Skip(page * 100).Take(100).ToList();
            return Ok(paginatedLocations);
        }

        // GET: api/v1/location/warehouse/{warehouseId}
        // Haal alle locaties op die in een specifieke warehouse zitten
        [HttpGet("warehouse/{warehouseId}")]
        public async Task<ActionResult<List<Location>>> GetLocationsInWarehouse(int warehouseId)
        {
            return await _locationService.GetLocationWarehouse(warehouseId); // 200 met locaties in een warehouse
        }

        // POST: api/v1/location
        // Voeg een nieuwe locatie toe
        [HttpPost]
        public async Task<ActionResult<string>> AddLocation([FromBody] Location location)
        {
            var result = await _locationService.AddLocation(location);

            if (result.Contains("Invalid"))
            {
                return BadRequest(result); // 400 als er iets mis is, bijvoorbeeld een ongeldige WarehouseId
            }
            return Ok(result); // 200 met een succesbericht
        }

        // PUT: api/v1/location/{id}
        // Update een bestaande locatie
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateLocation(int id, [FromBody] Location location)
        {
            var result = await _locationService.UpdateLocation(id, location);
            return Ok(result); // 200 met een succesbericht
        }

        // DELETE: api/v1/location/{id}
        // Verwijder een locatie op basis van ID
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> RemoveLocation(int id)
        {
            
            var result = await _locationService.RemoveLocation(id);
            if (!result)
            {
                return NotFound("Location does not exist");
            }
            return Ok(result); // 200 met een succesbericht
        }
    }
}

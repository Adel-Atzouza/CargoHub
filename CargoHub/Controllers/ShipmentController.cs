using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Controllers
{
[Route("api/v1/[controller]")]
[ApiController]
public class ShipmentController : ControllerBase
{
    private readonly ShipmentService _shipmentService;

    public ShipmentController(ShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Shipment>> GetShipmentById(int id)
    {
        var shipment = await _shipmentService.GetShipmentByIdAsync(id);

        if (shipment == null)
        {
            return NotFound($"Shipment with ID {id} not found.");
        }
        return Ok(shipment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShipment(int id){
        var shipment = await _shipmentService.DeleteShipment(id);

        if (!shipment)
        {
            return NotFound($"shipment with {id} does not exist");
        }
        return Ok($"Order with ID {id} deleted successfully.");
    }
}
}

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
        var shipment = await _shipmentService.GetShipmentID(id);
        if (shipment == null) return NotFound("Shipment not found.");
        return Ok(shipment);
    }

    [HttpGet]
    public async Task<ActionResult<List<Shipment>>> GetShipments()
    {
        return await _shipmentService.GetShipmentswithdetails();
    }

    [HttpPost]
    public async Task<ActionResult<string>> AddShipment([FromBody] ShipmentRequest shipmentRequest)
    {
        var result = await _shipmentService.AddShipment(shipmentRequest);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<string>> UpdateShipment(int id, [FromBody] ShipmentRequest shipmentRequest)
    {
        var result = await _shipmentService.UpdateShipment(id, shipmentRequest);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> RemoveShipment(int id)
    {
        var result = await _shipmentService.RemoveShipment(id);
        return Ok(result);
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<List<Shipment>>> GetShipmentsByShipmentDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var shipments = await _shipmentService.GetShipmentsByShipmentDateRange(startDate, endDate);
        return Ok(shipments);
    }
}

}

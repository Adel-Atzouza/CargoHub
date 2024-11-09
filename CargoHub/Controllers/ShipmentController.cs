using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Controllers
{
    [Route("api/[controller]")]
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
            var shipment = await _shipmentService.GetShipment(id);
            if (shipment == null) return NotFound("Shipment not found.");
            return Ok(shipment);
        }

        [HttpGet]
        public async Task<ActionResult<List<Shipment>>> GetShipments()
        {
            return await _shipmentService.GetShipments();
        }

        [HttpGet("{shipmentId}/items")]
        public async Task<ActionResult<List<Item>>> GetItemsInShipment(int shipmentId)
        {
            var items = await _shipmentService.GetItemsInShipment(shipmentId);
            if (items == null) return NotFound("Items not found in the shipment.");
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddShipment([FromBody] Shipment shipment)
        {
            var result = await _shipmentService.AddShipment(shipment);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateShipment(int id, [FromBody] Shipment shipment)
        {
            var result = await _shipmentService.UpdateShipment(id, shipment);
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

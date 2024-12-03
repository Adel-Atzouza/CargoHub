using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Controllers
{
    [Route("api/v1/Shipments")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly ShipmentService _shipmentService;

        // Constructor: Inject de service die alle shipment-logica regelt
        public ShipmentController(ShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ShipmentDTO>>> GetAllShipmentsWithItems(int page = 0)
        {
            var shipments = await _shipmentService.GetAllShipmentsWithItems();
            var paginatedShipments = shipments.Skip(page * 100).Take(100).ToList();
            return Ok(paginatedShipments); // Verander de return type naar List<ShipmentDTO>
        }

        // GET: api/v1/shipment/Orderdetails/{id}
        // Haal een shipment op met alle orders en hun details
        [HttpGet("Orderdetails/{id}")]
        public async Task<ActionResult<ShipmentDTO>> GetShipmentByIdWithOrders(int id)
        {
            var shipment = await _shipmentService.GetShipmentByIdWithOrderDetails(id);

            if (shipment == null)
            {
                return NotFound($"Shipment met ID {id} niet gevonden."); // 404 als de shipment er niet is
            }
            return Ok(shipment); // Verander de return type naar ShipmentDTO
        }

        // GET: api/v1/shipment/{id}
        // Haal alleen de items op voor een specifieke shipment
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetShipmentByIdItems(int id)
        {
            var shipment = await _shipmentService.GetShipmentItems(id);

            if (shipment == null)
            {
                return NotFound($"Shipment met ID {id} niet gevonden."); // 404 als de shipment er niet is
            }
            return Ok(shipment); // Hier blijft het object, want je retourneert de items
        }

        // POST: api/v1/shipment
        // Maak een nieuwe shipment aan
        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] Shipment shipment)
        {
            if (shipment == null)
            {
                return BadRequest("Shipmentgegevens zijn verplicht."); // 400 als er geen data is
            }
            var createdShipment = await _shipmentService.CreateShipment(shipment);
            return CreatedAtAction(nameof(GetShipmentByIdWithOrders), new { id = createdShipment.Id }, createdShipment); // 201 met de aangemaakte shipment
        }

        // PUT: api/v1/shipment/{shipmentId}/assign-orders
        // Wijs een lijst van orders toe aan een shipment
        [HttpPut("{shipmentId}/assign-orders")]
        public async Task<IActionResult> AssignOrdersToShipment(int shipmentId, [FromBody] List<int> orderIds)
        {
            if (orderIds == null || !orderIds.Any())
            {
                return BadRequest("Order-IDs zijn verplicht."); // 400 als er geen order-IDs zijn
            }

            try
            {
                var result = await _shipmentService.AssignOrdersToShipment(shipmentId, orderIds);

                if (!result)
                {
                    return NotFound($"Shipment met ID {shipmentId} niet gevonden of één of meer orders bestaan niet."); // 404 als de shipment of orders niet bestaan
                }

                return Ok($"Orders succesvol toegewezen aan shipment {shipmentId}."); // Alles gelukt
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}"); // 500 als er iets misgaat
            }
        }

        // PUT: api/v1/shipment/{shipmentId}/orders
        // Werk de lijst van orders in een shipment bij
        [HttpPut("{shipmentId}/orders")]
        public async Task<IActionResult> UpdateOrdersInShipment(int shipmentId, [FromBody] List<int> orderIds)
        {
            if (orderIds == null || !orderIds.Any())
            {
                return BadRequest("Order-IDs zijn verplicht."); // 400 als er geen order-IDs zijn
            }

            try
            {
                var result = await _shipmentService.UpdateOrdersInShipment(shipmentId, orderIds);
                if (!result)
                {
                    return NotFound($"Shipment met ID {shipmentId} niet gevonden."); // 404 als de shipment er niet is
                }

                return Ok(); // succes
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}"); // 500 bij fouten
            }
        }

        // DELETE: api/v1/shipment/{id}
        // Verwijder een shipment via het ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var shipment = await _shipmentService.DeleteShipment(id);

            if (!shipment)
            {
                return NotFound($"Shipment met ID {id} bestaat niet."); // 404 als de shipment niet bestaat
            }
            return Ok($"Shipment met ID {id} succesvol verwijderd."); // Alles gelukt
        }
    }
}

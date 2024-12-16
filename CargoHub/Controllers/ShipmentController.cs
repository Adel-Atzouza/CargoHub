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

        public ShipmentController(ShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet("{page}")]
        public async Task<ActionResult<List<ShipmentDTO>>> GetAllShipmentsWithOrderItems(int page = 0)
        {
            var shipments = await _shipmentService.GetAllShipmentsWithorderdetailsItems();
            var paginatedShipments = shipments.Skip(page * 100).Take(100).ToList();
            return Ok(paginatedShipments);
        }

        [HttpGet("Orderdetails/{id}")]
        public async Task<ActionResult<ShipmentDTO>> GetShipmentByIdWithOrders(int id)
        {
            var shipment = await _shipmentService.GetShipmentByIdWithOrderDetails(id);

            if (shipment == null)
            {
                return NotFound($"Shipment met ID {id} niet gevonden.");
            }
            return Ok(shipment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetShipmentByIdItems(int id)
        {
            var shipment = await _shipmentService.GetShipmentItems(id);

            if (shipment == null)
            {
                return NotFound($"Shipment met ID {id} niet gevonden.");
            }
            return Ok(shipment);
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetAllShipmentByIdItems()
        {
            var shipment = await _shipmentService.GetAllShipmentsWithItems();

            if (shipment == null)
            {
                return NotFound("Geen Shipment gevonden.");
            }
            return Ok(shipment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] Shipment shipment)
        {
            if (shipment == null)
            {
                return BadRequest("Shipmentgegevens zijn verplicht.");
            }
            var createdShipment = await _shipmentService.CreateShipment(shipment);
            return CreatedAtAction(nameof(GetShipmentByIdWithOrders), new { id = createdShipment.Id }, createdShipment);
        }

        [HttpPut("{shipmentId}/assign-orders")]
        public async Task<IActionResult> AssignOrdersToShipment(int shipmentId, [FromBody] List<int> orderIds)
        {
            if (orderIds == null || !orderIds.Any())
            {
                return BadRequest("Order-IDs zijn verplicht.");
            }

            try
            {
                var result = await _shipmentService.AssignOrdersToShipment(shipmentId, orderIds);

                if (!result)
                {
                    return NotFound($"Shipment met ID {shipmentId} niet gevonden of één of meer orders bestaan niet.");
                }

                return Ok($"Orders succesvol toegewezen aan shipment {shipmentId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }

        [HttpPut("{shipmentId}/orders")]
        public async Task<IActionResult> UpdateOrdersInShipment(int shipmentId, [FromBody] List<int> orderIds)
        {
            if (orderIds == null || !orderIds.Any())
            {
                return BadRequest("Order-IDs zijn verplicht.");
            }

            try
            {
                var result = await _shipmentService.UpdateOrdersInShipment(shipmentId, orderIds);
                if (!result)
                {
                    return NotFound($"Shipment met ID {shipmentId} niet gevonden.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }

        [HttpPut("{shipmentId}")]
        public async Task<IActionResult> UpdateShipmentFields(int shipmentId, [FromBody] ShipmentDTO updatedShipmentDto)
        {
            if (updatedShipmentDto == null)
            {
                return BadRequest("Shipment data mag niet null zijn.");
            }

            try
            {
                var result = await _shipmentService.UpdateShipmentFields(shipmentId, updatedShipmentDto);

                if (result.Contains("niet gevonden"))
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Er is een fout opgetreden: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var shipment = await _shipmentService.DeleteShipment(id);

            if (!shipment)
            {
                return NotFound($"Shipment met ID {id} bestaat niet.");
            }
            return Ok($"Shipment met ID {id} succesvol verwijderd.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CargoHub.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        // Constructor: Inject hier gewoon de service die alle orderlogica regelt
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/v1/orders
        // Haal alle orders op, inclusief hun items
        [HttpGet]
        public async Task<ActionResult<List<OrderWithItemsDTO>>> GetAllOrders([FromQuery] int page = 0)
        {
            var orderwithitems = await _orderService.GetAllOrdersWithItems();
            var paginatedorders = orderwithitems.Skip(page * 100).Take(100).ToList();
            return Ok(paginatedorders);
            // var orders = await _orderService.GetAllOrdersWithItems();
            // return Ok(orders); //lijst met orders
        }

        // GET: api/v1/orders/{id}
        // Haal een specifieke order op (inclusief items) via het ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderWithItemsDTO>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderWithItems(id);
            if (order == null)
            {
                return NotFound($"Order met ID {id} niet gevonden."); // 404 als we niets vinden
            }
            return Ok(order); // Alles goed
        }

        // GET: api/v1/orders/client/{clientId}
        // Haal alle orders van een specifieke klant op via hun ID
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<List<Order>>> GetOrdersForClient(int clientId)
        {
            var orders = await _orderService.GetOrdersForClient(clientId);

            if (orders == null || orders.Count == 0)
            {
                return NotFound(); // Geen orders
            }

            return Ok(orders); // Alles gevonden
        }

        // POST: api/v1/orders
        // Maak een nieuwe order aan met items
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderWithItemsDTO orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Ordergegevens zijn verplicht."); // Geen gegevens
            }

            try
            {
                //alles netjes mappen naar een Order-object
                var order = new Order
                {
                    SourceId = orderDto.SourceId,
                    OrderDate = orderDto.OrderDate,
                    RequestDate = orderDto.RequestDate,
                    Reference = orderDto.Reference,
                    ExtrReference = orderDto.ReferenceExtra,
                    OrderStatus = orderDto.OrderStatus,
                    Notes = orderDto.Notes,
                    ShippingNotes = orderDto.ShippingNotes,
                    PickingNotes = orderDto.PickingNotes,
                    WarehouseId = orderDto.WarehouseId,
                    ShipTo = orderDto.ShipTo ?? null, // Nullable veld
                    BillTo = orderDto.BillTo ?? null, // Idem voor BillTo
                    ShipmentId = orderDto.ShipmentId ?? null, // En ShipmentId ook
                    TotalAmount = orderDto.TotalAmount,
                    TotalDiscount = orderDto.TotalDiscount,
                    TotalTax = orderDto.TotalTax,
                    TotalSurcharge = orderDto.TotalSurcharge,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Laat de service de order aanmaken
                var createdOrder = await _orderService.CreateOrder(order, orderDto.Items);

                // Klaar, retourneet de order
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                //iets is er verkeerd gegaan. dit is voor debuggen
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }

        // PUT: api/v1/orders/{id}/items
        // Werk de items in een bestaande order bij
        [HttpPut("{id}/items")]
        public async Task<IActionResult> UpdateItemsInOrder(int id, [FromBody] List<ItemDTO> orderItems)
        {
            if (orderItems == null || orderItems.Count == 0)
            {
                return BadRequest("De lijst met items mag niet leeg zijn.");
            }

            try
            {
                // Laat de service de items in de order bijwerken
                var result = await _orderService.UpdateItemsInOrder(id, orderItems);

                if (result.StartsWith("Item met ID") || result.StartsWith("Order met ID") || result.StartsWith("Niet genoeg voorraad"))
                {
                    return BadRequest(result); // Fouten zoals ontbrekende items, orders of voorraadtekort
                }

                return Ok($"Order met ID {id} is succesvol bijgewerkt.");
            }
            catch (Exception ex)
            {
                // Als er een onverwachte fout optreedt
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderWithItemsDTO updatedOrderDto)
        {
            if (updatedOrderDto == null)
            {
                return BadRequest("De ordergegevens zijn verplicht.");
            }

            var result = await _orderService.UpdateOrder(id, updatedOrderDto);

            if (result.StartsWith("Order met ID"))
            {
                return Ok(result);
            }

            return BadRequest(result);
        }



        // DELETE: api/v1/orders/{id}
        // Verwijder een order via het ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                // Laat de service de order verwijderen
                var result = await _orderService.DeleteOrder(id);
                if (!result)
                {
                    return NotFound($"Order met ID {id} niet gevonden."); // 404 als er niets is
                }

                return Ok($"Order met ID {id} succesvol verwijderd."); // Alles goed
            }
            catch (Exception ex)
            {
                //iets is er verkeerd gegaan. dit is voor debuggen
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }
    }
}

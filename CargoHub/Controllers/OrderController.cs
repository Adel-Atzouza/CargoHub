using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CargoHub.Models;
using CargoHub.Services;

namespace CargoHub.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderWithItemsDTO>>> GetAllOrders([FromQuery] int page = 0)
        {
            var orderwithitems = await _orderService.GetAllOrdersWithItems();
            var paginatedorders = orderwithitems.Skip(page * 100).Take(100).ToList();
            return Ok(paginatedorders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderWithItemsDTO>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderWithItems(id);
            if (order == null)
            {
                return NotFound($"Order met ID {id} niet gevonden.");
            }
            return Ok(order);
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<List<Order>>> GetOrdersForClient(int clientId)
        {
            var orders = await _orderService.GetOrdersForClient(clientId);

            if (orders == null || orders.Count == 0)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpGet("warehouse/{warehouseId}")]
        public async Task<ActionResult<List<Order>>> GetOrderForWarehouse(int warehouseId)
        {
            var orders = await _orderService.GetOrdersLinkedWithWarehouseId(warehouseId);

            if (orders == null || orders.Count == 0)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("source/{sourceId}")]
        public async Task<ActionResult<List<Order>>> GetOrderForSource(int sourceId)
        {
            var orders = await _orderService.GetOrdersLinkedWithSourceId(sourceId);

            if (orders == null || orders.Count == 0)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<Order>>> GetOrderForStatus(string status)
        {
            var orders = await _orderService.GetOrdersStatus(status);

            if (orders == null || orders.Count == 0)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderWithItemsDTO orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Ordergegevens zijn verplicht.");
            }

            try
            {
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
                    WarehouseId = orderDto.WarehouseId ?? null,
                    ShipTo = orderDto.ShipTo ?? null,
                    BillTo = orderDto.BillTo ?? null,
                    ShipmentId = orderDto.ShipmentId ?? null,
                    TotalAmount = orderDto.TotalAmount,
                    TotalDiscount = orderDto.TotalDiscount,
                    TotalTax = orderDto.TotalTax,
                    TotalSurcharge = orderDto.TotalSurcharge,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdOrder = await _orderService.CreateOrder(order, orderDto.Items);
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }

        [HttpPut("{id}/items")]
        public async Task<IActionResult> UpdateItemsInOrder(int id, [FromBody] List<ItemDTO> orderItems)
        {
            if (orderItems == null || orderItems.Count == 0)
            {
                return BadRequest("De lijst met items mag niet leeg zijn.");
            }

            try
            {
                var result = await _orderService.UpdateItemsInOrder(id, orderItems);

                if (result.StartsWith("Item met ID") || result.StartsWith("Order met ID") || result.StartsWith("Niet genoeg voorraad"))
                {
                    return BadRequest(result);
                }

                return Ok($"Order met ID {id} is succesvol bijgewerkt.");
            }
            catch (Exception ex)
            {
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrder(id);
                if (!result)
                {
                    return NotFound($"Order met ID {id} niet gevonden.");
                }

                return Ok($"Order met ID {id} succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }
    }
}

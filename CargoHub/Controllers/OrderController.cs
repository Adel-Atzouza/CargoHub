using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargoHub.Models;
using CargoHub.Services;

namespace CargoHub.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/v1/orders
        [HttpGet]
        public async Task<ActionResult<List<OrderWithItemsDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersWithItems();
            return Ok(orders);
        }

        // GET: api/v1/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderWithItemsDTO>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderWithItems(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/v1/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderWithItemsDTO orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest();
            }

            // Map DTO to Order
            var order = new Order
            {
                SourceId = orderDto.SourceId,
                OrderDate = orderDto.OrderDate,
                RequestDate = orderDto.RequestDate,
                Reference = orderDto.Reference,
                ExtraReference = orderDto.ReferenceExtra,
                OrderStatus = orderDto.OrderStatus,
                Notes = orderDto.Notes,
                ShippingNotes = orderDto.ShippingNotes,
                PickingNotes = orderDto.PickingNotes,
                WarehouseId = orderDto.WarehouseId,
                ShipTo = (int)orderDto.ShipTo,
                BillTo = (int)orderDto.BillTo,
                ShipmentId = orderDto.ShipmentId ?? 0,
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

        // PUT: api/v1/orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderWithItemsDTO orderDto)
        {
            if (orderDto == null || id != orderDto.Id)
            {
                return BadRequest();
            }

            var order = new Order
            {
                Id = id,
                SourceId = orderDto.SourceId,
                OrderDate = orderDto.OrderDate,
                RequestDate = orderDto.RequestDate,
                Reference = orderDto.Reference,
                ExtraReference = orderDto.ReferenceExtra,
                OrderStatus = orderDto.OrderStatus,
                Notes = orderDto.Notes,
                ShippingNotes = orderDto.ShippingNotes,
                PickingNotes = orderDto.PickingNotes,
                WarehouseId = orderDto.WarehouseId,
                ShipTo = (int)orderDto.ShipTo,
                BillTo = (int)orderDto.BillTo,
                ShipmentId = orderDto.ShipmentId ?? 0,
                TotalAmount = orderDto.TotalAmount,
                TotalDiscount = orderDto.TotalDiscount,
                TotalTax = orderDto.TotalTax,
                TotalSurcharge = orderDto.TotalSurcharge,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _orderService.UpdateOrder(id, order, orderDto.Items);
            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/v1/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

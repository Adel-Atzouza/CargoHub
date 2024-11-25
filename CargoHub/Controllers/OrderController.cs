using Microsoft.AspNetCore.Mvc;
using System;
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

        // Constructor to inject the OrderService dependency
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/v1/orders
        // Retrieves all orders with their associated items
        [HttpGet]
        public async Task<ActionResult<List<OrderWithItemsDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersWithItems();
            return Ok(orders); // Return the list of orders in the response
        }

        // GET: api/v1/orders/{id}
        // Retrieves a specific order with its associated items by order ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderWithItemsDTO>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderWithItems(id);
            if (order == null)
            {
                return NotFound(); // Return 404 if the order is not found
            }
            return Ok(order); // Return the order with a 200 OK status
        }

        // GET: api/v1/orders/client/{clientId}
        // Retrieves all orders associated with a specific client by client ID
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<List<Order>>> GetOrdersForClient(int clientId)
        {
            var orders = await _orderService.GetOrdersCLient(clientId);

            if (orders == null || orders.Count == 0)
            {
                return NotFound(); // Return 404 if no orders are found for the client
            }

            return Ok(orders); // Return the list of orders for the client
        }

        // POST: api/v1/orders
        // Creates a new order with associated items
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderWithItemsDTO orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Order data is required."); // Return 400 if the request body is null
            }

            try
            {
                // Map the DTO to the Order entity
                var order = new Order
                {
                    SourceId = orderDto.SourceId,
                    OrderDate = orderDto.OrderDate,
                    RequestDate = orderDto.RequestDate,
                    Reference = orderDto.Reference,
                    ExtrReference = orderDto.ReferenceExtra, // Map extra reference field
                    OrderStatus = orderDto.OrderStatus,
                    Notes = orderDto.Notes,
                    ShippingNotes = orderDto.ShippingNotes,
                    PickingNotes = orderDto.PickingNotes,
                    WarehouseId = orderDto.WarehouseId,
                    ShipTo = orderDto.ShipTo ?? null, // Handle nullable ShipTo
                    BillTo = orderDto.BillTo ?? null, // Handle nullable BillTo
                    ShipmentId = orderDto.ShipmentId ?? null, // Handle nullable ShipmentId
                    TotalAmount = orderDto.TotalAmount,
                    TotalDiscount = orderDto.TotalDiscount,
                    TotalTax = orderDto.TotalTax,
                    TotalSurcharge = orderDto.TotalSurcharge,
                    CreatedAt = DateTime.UtcNow, // Set creation timestamp
                    UpdatedAt = DateTime.UtcNow // Set update timestamp
                };

                // Call the service to create the order
                var createdOrder = await _orderService.CreateOrder(order, orderDto.Items);

                // Return a 201 Created response with the created order
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error if something goes wrong
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/v1/orders/{id}
        // Updates an existing order with new data
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderWithItemsDTO orderDto)
        {
            if (orderDto == null || id != orderDto.Id)
            {
                return BadRequest("Order ID mismatch."); // Return 400 if the IDs don't match
            }

            try
            {
                // Map the DTO to the Order entity
                var order = new Order
                {
                    Id = id, // Ensure the correct order ID is set
                    SourceId = orderDto.SourceId,
                    OrderDate = orderDto.OrderDate,
                    RequestDate = orderDto.RequestDate,
                    Reference = orderDto.Reference,
                    ExtrReference = orderDto.ReferenceExtra, // Map extra reference field
                    OrderStatus = orderDto.OrderStatus,
                    Notes = orderDto.Notes,
                    ShippingNotes = orderDto.ShippingNotes,
                    PickingNotes = orderDto.PickingNotes,
                    WarehouseId = orderDto.WarehouseId,
                    ShipTo = orderDto.ShipTo ?? null, // Handle nullable ShipTo
                    BillTo = orderDto.BillTo ?? null, // Handle nullable BillTo
                    ShipmentId = orderDto.ShipmentId ?? null, // Handle nullable ShipmentId
                    TotalAmount = orderDto.TotalAmount,
                    TotalDiscount = orderDto.TotalDiscount,
                    TotalTax = orderDto.TotalTax,
                    TotalSurcharge = orderDto.TotalSurcharge,
                    UpdatedAt = DateTime.UtcNow // Update the timestamp
                };

                // Call the service to update the order
                var updatedOrder = await _orderService.UpdateOrder(id, order, orderDto.Items);
                if (updatedOrder == null)
                {
                    return NotFound(); // Return 404 if the order is not found
                }

                return NoContent(); // Return 204 No Content to indicate success
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error if something goes wrong
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/v1/orders/{id}
        // Deletes an order by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                // Call the service to delete the order
                var result = await _orderService.DeleteOrder(id);
                if (!result)
                {
                    return NotFound(); // Return 404 if the order is not found
                }

                return Ok($"Order with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error if something goes wrong
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

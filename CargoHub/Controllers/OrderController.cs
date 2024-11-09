using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByID(id);
            if (order == null) return NotFound("Order not found.");
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders([FromQuery] int id)
        {
            return await _orderService.GetOrders(id);
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddOrder([FromBody] Order order)
        {
            var result = await _orderService.AddOrder(order);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateOrder(int id, [FromBody] Order order)
        {
            var result = await _orderService.UpdateOrder(id, order);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);
            return Ok(result);
        }
    }
}

using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var order = new Order
        {
            SourceId = orderDto.SourceId,
            OrderDate = orderDto.OrderDate,
            RequestDate = orderDto.RequestDate,
            Reference = orderDto.Reference,
            ReferenceExtra = orderDto.ReferenceExtra,
            OrderStatus = orderDto.OrderStatus,
            Notes = orderDto.Notes,
            ShippingNotes = orderDto.ShippingNotes,
            PickingNotes = orderDto.PickingNotes,
            WarehouseId = orderDto.WarehouseId,
            ShipTo = orderDto.ShipTo,
            BillTo = orderDto.BillTo,
            ShipmentId = orderDto.ShipmentId,
            TotalAmount = orderDto.TotalAmount,
            TotalDiscount = orderDto.TotalDiscount,
            TotalTax = orderDto.TotalTax,
            TotalSurcharge = orderDto.TotalSurcharge,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Items = orderDto.Items.Select(i => new OrderItem
            {
                ItemId = i.ItemId, // Now uses string
                Amount = i.Amount
            }).ToList()
        };

        await _orderService.CreateOrderAsync(order);

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        var result = new OrderResponseDto
        {
            Id = order.Id,
            SourceId = order.SourceId,
            OrderDate = order.OrderDate,
            RequestDate = order.RequestDate,
            Reference = order.Reference,
            ReferenceExtra = order.ReferenceExtra,
            OrderStatus = order.OrderStatus,
            Notes = order.Notes,
            ShippingNotes = order.ShippingNotes,
            PickingNotes = order.PickingNotes,
            WarehouseId = order.WarehouseId,
            ShipTo = order.ShipTo,
            BillTo = order.BillTo,
            ShipmentId = order.ShipmentId,
            TotalAmount = order.TotalAmount,
            TotalDiscount = order.TotalDiscount,
            TotalTax = order.TotalTax,
            TotalSurcharge = order.TotalSurcharge,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Items = order.Items.Select(i => new OrderItemResponseDto
            {
                ItemId = i.ItemId,
                Amount = i.Amount
            }).ToList()
        };

        return Ok(result);
    }
}

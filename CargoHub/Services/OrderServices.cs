using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CargoHub.Models;

namespace CargoHub.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // Method to get a specific order with items by order ID
        public async Task<OrderWithItemsDTO> GetOrderWithItems(int id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new OrderWithItemsDTO
                {
                    Id = o.Id,
                    SourceId = o.SourceId,
                    OrderDate = o.OrderDate,
                    RequestDate = o.RequestDate,
                    Reference = o.Reference,
                    ReferenceExtra = o.ExtrReference,
                    OrderStatus = o.OrderStatus,
                    Notes = o.Notes,
                    ShippingNotes = o.ShippingNotes,
                    PickingNotes = o.PickingNotes,
                    WarehouseId = o.WarehouseId,
                    ShipTo = o.ShipTo,
                    BillTo = o.BillTo,
                    ShipmentId = o.ShipmentId,
                    TotalAmount = o.TotalAmount,
                    TotalDiscount = o.TotalDiscount,
                    TotalTax = o.TotalTax,
                    TotalSurcharge = o.TotalSurcharge,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    Items = o.OrderItems.Select(oi => new ItemDTO
                    {
                        ItemId = oi.Item.Uid, // Fetching Uid as a string
                        Amount = oi.Amount
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return order;
        }

        // Method to get all orders with items
        public async Task<List<OrderWithItemsDTO>> GetAllOrdersWithItems()
        {
            return await _context.Orders
                .Select(o => new OrderWithItemsDTO
                {
                    Id = o.Id,
                    SourceId = o.SourceId,
                    OrderDate = o.OrderDate,
                    RequestDate = o.RequestDate,
                    Reference = o.Reference,
                    ReferenceExtra = o.ExtrReference,
                    OrderStatus = o.OrderStatus,
                    Notes = o.Notes,
                    ShippingNotes = o.ShippingNotes,
                    PickingNotes = o.PickingNotes,
                    WarehouseId = o.WarehouseId,
                    ShipTo = o.ShipTo,
                    BillTo = o.BillTo,
                    ShipmentId = o.ShipmentId,
                    TotalAmount = o.TotalAmount,
                    TotalDiscount = o.TotalDiscount,
                    TotalTax = o.TotalTax,
                    TotalSurcharge = o.TotalSurcharge,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    Items = o.OrderItems.Select(oi => new ItemDTO
                    {
                        ItemId = oi.Item.Uid, // Fetching Uid as a string
                        Amount = oi.Amount
                    }).ToList()
                })
                .ToListAsync();
        }

        // Method to create a new order with items
public async Task<Order> CreateOrder(Order order, List<ItemDTO> itemDTOs)
    {
        // Add the order first to generate the ID
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(); // Save to get the order's Id

        // Attach items to the order
        foreach (var itemDto in itemDTOs)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);  // Use Uid to find the item
            if (item != null)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id, // Set the OrderId correctly here
                    ItemId = item.Uid,   // Use item.Uid, since ItemId is now a string
                    Amount = itemDto.Amount
                };
                _context.OrderItems.Add(orderItem); // Add the orderItem to the context
            }
        }

        await _context.SaveChangesAsync(); // Save changes after adding items
        return order;
    }

    // Method to update an existing order
    public async Task<Order> UpdateOrder(int id, Order updatedOrder, List<ItemDTO> updatedItemDTOs)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return null;

        // Update the order properties
        order.SourceId = updatedOrder.SourceId;
        order.OrderDate = updatedOrder.OrderDate;
        order.RequestDate = updatedOrder.RequestDate;
        order.Reference = updatedOrder.Reference;
        order.ExtrReference = updatedOrder.ExtrReference;
        order.OrderStatus = updatedOrder.OrderStatus;
        order.Notes = updatedOrder.Notes;
        order.ShippingNotes = updatedOrder.ShippingNotes;
        order.PickingNotes = updatedOrder.PickingNotes;
        order.WarehouseId = updatedOrder.WarehouseId;
        order.ShipTo = updatedOrder.ShipTo;
        order.BillTo = updatedOrder.BillTo;
        order.ShipmentId = updatedOrder.ShipmentId;
        order.TotalAmount = updatedOrder.TotalAmount;
        order.TotalDiscount = updatedOrder.TotalDiscount;
        order.TotalTax = updatedOrder.TotalTax;
        order.TotalSurcharge = updatedOrder.TotalSurcharge;
        order.UpdatedAt = DateTime.UtcNow;

        // Update order items
        _context.OrderItems.RemoveRange(order.OrderItems); // Remove old items
        foreach (var itemDto in updatedItemDTOs)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);  // Use Uid to find the item
            if (item != null)
            {
                var orderItem = new OrderItem
                {
                    OrderId = id,
                    ItemId = item.Uid,  // Use item.Uid as ItemId is now a string
                    Amount = itemDto.Amount
                };
                _context.OrderItems.Add(orderItem); // Add new items
            }
        }

        await _context.SaveChangesAsync();
        return order;
    }

        // Method to delete an order by ID
        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

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
                    ReferenceExtra = o.ExtrReference, // Correct field name for ReferenceExtra
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
                        ItemId = oi.Item.Uid,
                        Amount = oi.Amount
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<List<Order>> GetOrdersCLient(int id)
        {
            return await _context.Orders
                .Where(c => c.BillTo == id || c.ShipTo == id)
                .ToListAsync();
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
                    ReferenceExtra = o.ExtrReference, // Correct field name for ReferenceExtra
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
                        ItemId = oi.Item.Uid,
                        Amount = oi.Amount
                    }).ToList()
                })
                .ToListAsync();
        }

        // Method to create a new order with items
        public async Task<Order> CreateOrder(Order order, List<ItemDTO> itemDTOs)
        {
            try
            {
                // Add the order first to generate the ID
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Save to get the order's Id

                // Attach items to the order
                foreach (var itemDto in itemDTOs)
                {
                    var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);
                    if (item != null)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id, // Set the OrderId correctly here
                            ItemId = item.Id,   // Use item.Id (int) as ItemId
                            Amount = itemDto.Amount
                        };
                        _context.OrderItems.Add(orderItem); // Add the orderItem to the context
                    }
                    else
                    {
                        throw new Exception($"Item with Uid {itemDto.ItemId} does not exist.");
                    }
                }

                await _context.SaveChangesAsync(); // Save changes after adding items
                return order;
            }
            catch (Exception ex)
            {
                // Log the error and rethrow for handling in the controller
                Console.WriteLine($"Error creating order: {ex.Message}");
                throw;
            }
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
            order.ExtrReference = updatedOrder.ExtrReference; // Fixed field name
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

            // Remove old items and add the new ones
            _context.OrderItems.RemoveRange(order.OrderItems); // Remove old items
            foreach (var itemDto in updatedItemDTOs)
            {
                var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);
                if (item != null)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = id,
                        ItemId = item.Id,  // Use item.Id (int) as ItemId
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

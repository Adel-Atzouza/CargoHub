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

        // Constructor to inject the AppDbContext dependency
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // Method to get a specific order with items by order ID
        public async Task<OrderWithItemsDTO> GetOrderWithItems(int id)
        {
            // Query the database to retrieve a specific order by ID with its related items
            var order = await _context.Orders
                .Where(o => o.Id == id) // Filter by order ID
                .Select(o => new OrderWithItemsDTO
                {
                    Id = o.Id,
                    SourceId = o.SourceId,
                    OrderDate = o.OrderDate,
                    RequestDate = o.RequestDate,
                    Reference = o.Reference,
                    ReferenceExtra = o.ExtrReference, // Map the correct field for ReferenceExtra
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
                        ItemId = oi.Item.Uid, // Map the Item's unique identifier
                        Amount = oi.Amount // Map the quantity of the item
                    }).ToList()
                })
                .FirstOrDefaultAsync(); // Get the first matching record or null if not found

            return order; // Return the order with its details
        }

        // Method to get all orders for a specific client
        public async Task<List<Order>> GetOrdersCLient(int id)
        {
            // Retrieve all orders where the client is either the bill-to or ship-to party
            return await _context.Orders
                .Where(c => c.BillTo == id || c.ShipTo == id)
                .ToListAsync(); // Return the list of matching orders
        }

        // Method to get all orders with their items
        public async Task<List<OrderWithItemsDTO>> GetAllOrdersWithItems()
        {
            // Query all orders with their related items
            return await _context.Orders
                .Select(o => new OrderWithItemsDTO
                {
                    Id = o.Id,
                    SourceId = o.SourceId,
                    OrderDate = o.OrderDate,
                    RequestDate = o.RequestDate,
                    Reference = o.Reference,
                    ReferenceExtra = o.ExtrReference, // Map the correct field for ReferenceExtra
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
                        ItemId = oi.Item.Uid, // Map the Item's unique identifier
                        Amount = oi.Amount // Map the quantity of the item
                    }).ToList()
                })
                .ToListAsync(); // Return the list of orders with items
        }

        // Method to create a new order with items
        public async Task<Order> CreateOrder(Order order, List<ItemDTO> itemDTOs)
        {
            try
            {
                // Add the order to the database
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Save to generate the order ID

                // Attach items to the order
                foreach (var itemDto in itemDTOs)
                {
                    // Find the item in the database by its unique identifier
                    var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);
                    if (item != null)
                    {
                        // Create an order-item association
                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id, // Set the order ID
                            ItemId = item.Id,   // Use the item's ID
                            Amount = itemDto.Amount // Set the quantity
                        };
                        _context.OrderItems.Add(orderItem); // Add the order-item association
                    }
                    else
                    {
                        throw new Exception($"Item with Uid {itemDto.ItemId} does not exist.");
                    }
                }

                await _context.SaveChangesAsync(); // Save changes to persist the order and items
                return order; // Return the created order
            }
            catch (Exception ex)
            {
                // Log the error and rethrow for handling in the controller
                Console.WriteLine($"Error creating order: {ex.Message}");
                throw;
            }
        }
        // public async Task<string> UpdateitemsInOrder(int orderid, List<ItemDTO> orderitems )
        // {
        //     foreach( var x in orderitems)
        //     {
        //         if(!await ItemExist(x.ItemId))
        //         {
        //             return $"item met id {x.ItemId} is not found";
        //         }
        //     }
        //     var Updateorder = await GetOrderWithItems(orderid);
        //     if(Updateorder == null ){
        //         return $"order with {orderid} not found";
        //     }

        //     var currentitems = Updateorder.Items;

        //     //STAP 1 het updaten van bestaande items in the huidige order
        //     foreach (var x in currentitems)
        //     {
        //         foreach(var y in orderitems)
        //         {

        //             if (x.ItemId == y.ItemId)
        //             {
        //                 var inventoryitem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == x.ItemId);
        //                 if(inventoryitem != null){
        //                     inventoryitem.TotalAllocated += y.Amount - x.Amount;
        //                 }
        //                 x.Amount = y.Amount;
        //                 break;
        //             }
        //         }
        //     }

        //     //STAP 2 het toevoegen van nieuwe items aan de huidige order
        //     foreach (var y in orderitems)
        //     {
        //         var existingItem = currentitems.FirstOrDefault(i => i.ItemId == y.ItemId);
        //         {

        //         }
        //     }

        // }

        // Method to update an existing order
        public async Task<Order> UpdateOrder(int id, Order updatedOrder, List<ItemDTO> updatedItemDTOs)
        {
            // Retrieve the existing order by ID
            var order = await _context.Orders
                .Include(o => o.OrderItems) // Include related order items
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null; // Return null if the order does not exist

            // Update order properties with new values
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
            order.UpdatedAt = DateTime.UtcNow; // Set the updated timestamp

            // Remove existing items and add new ones
            _context.OrderItems.RemoveRange(order.OrderItems); // Remove old items
            foreach (var itemDto in updatedItemDTOs)
            {
                // Find the new items in the database
                var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);
                if (item != null)
                {
                    // Create a new order-item association
                    var orderItem = new OrderItem
                    {
                        OrderId = id,
                        ItemId = item.Id,  // Use the item's ID
                        Amount = itemDto.Amount // Set the quantity
                    };
                    _context.OrderItems.Add(orderItem); // Add the new association
                }
            }

            await _context.SaveChangesAsync(); // Save changes
            return order; // Return the updated order
        }

        // Method to check if an item exists by its unique identifier
        public async Task<bool> ItemExist(string itemid)
        {
            return await _context.Items.AnyAsync(i => i.Uid == itemid);
        }

        // Method to delete an order by ID
        public async Task<bool> DeleteOrder(int id)
        {
            // Find the order by ID
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false; // Return false if the order does not exist

            // Remove the order from the database
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(); // Save changes to delete the order
            return true; // Return true to indicate successful deletion
        }
    }
}
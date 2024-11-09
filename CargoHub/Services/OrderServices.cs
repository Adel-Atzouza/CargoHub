using CargoHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CargoHub.Services{

    public class OrderService{
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Order>> GetOrders(int Id) //gives id and the it gives back the orders form that point and 100 further
        {
            return await _context.Orders
            .Where(order => order.Id >= Id)
            .OrderBy(order => order.Id)
            .Take(100)
            .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersDateTime(DateTime Starttime, DateTime EndTime)//gives a start time and a endTime and gives the orders in that range. 
        {
            return await _context.Orders
            .Where(order => order.Requestdate >= Starttime && order.Requestdate <= EndTime)
            .OrderBy(order => order.Requestdate)
            .ToListAsync();
        }

        public async Task<Order>? GetOrderByID(int id) // get a specific order
        {
            return await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<List<Order>> GetOrdersinShipment(int id)
        {
            return await _context.Orders
            .Where(order => order.ShipmentId == id)
            .ToListAsync();
        }

        public async Task<List<Order>> GetItemsinOrder(int id)
        {
            return await _context.Orders
            .Where(order => order.Id == id)
            .SelectMany(order => order.Itemlist)
            .ToListAsync();
        }
        public async Task<List<Order>> GetGetordersForCLient(Guid id)
        {
            return await _context.Orders
            .Where(order => order.ShipTo == id || order.BillTo == id)
            .ToListAsync();
        }

        public async Task<string> AddOrder(Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return "Location added successfully.";
        }

        public async Task<string> UpdateOrder(int orderId, Order updatedOrder)
        {
            if (updatedOrder.Id != orderId)
            {
                return "Error: Modifying the order ID is not allowed.";
            }

            updatedOrder.UpdatedAt = DateTime.UtcNow;
            var existingOrder = await _context.Orders.FindAsync(orderId);
            
            if (existingOrder != null)
            {
                _context.Entry(existingOrder).CurrentValues.SetValues(updatedOrder);
                await _context.SaveChangesAsync();
                return "Order updated successfully.";
            }
            else
            {
                return "Error: Order not found.";
            }
        }


        public async Task<string> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return "Error: Order not found.";
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return "Order deleted successfully.";
        }
    }
}
    // def add_order(self, order):
    //     order["created_at"] = self.get_timestamp()
    //     order["updated_at"] = self.get_timestamp()
    //     self.data.append(order)


    
    //     def update_items_in_order(self, order_id, items):
    //     order = self.get_order(order_id)
    //     current = order["items"]
    //     for x in current:
    //         found = False
    //         for y in items:
    //             if x["item_id"] == y["item_id"]:
    //                 found = True
    //                 break
    //         if not found:
    //             inventories = data_provider.fetch_inventory_pool().get_inventories_for_item(x["item_id"])
    //             min_ordered = 1_000_000_000_000_000_000
    //             min_inventory
    //             for z in inventories:
    //                 if z["total_allocated"] > min_ordered:
    //                     min_ordered = z["total_allocated"]
    //                     min_inventory = z
    //             min_inventory["total_allocated"] -= x["amount"]
    //             min_inventory["total_expected"] = y["total_on_hand"] + y["total_ordered"]
    //             data_provider.fetch_inventory_pool().update_inventory(min_inventory["id"], min_inventory)
    //     for x in current:
    //         for y in items:
    //             if x["item_id"] == y["item_id"]:
    //                 inventories = data_provider.fetch_inventory_pool().get_inventories_for_item(x["item_id"])
    //                 min_ordered = 1_000_000_000_000_000_000
    //                 min_inventory
    //                 for z in inventories:
    //                     if z["total_allocated"] < min_ordered:
    //                         min_ordered = z["total_allocated"]
    //                         min_inventory = z
    //             min_inventory["total_allocated"] += y["amount"] - x["amount"]
    //             min_inventory["total_expected"] = y["total_on_hand"] + y["total_ordered"]
    //             data_provider.fetch_inventory_pool().update_inventory(min_inventory["id"], min_inventory)
    //     order["items"] = items
    //     self.update_order(order_id, order)

    // def update_orders_in_shipment(self, shipment_id, orders):
    //     packed_orders = self.get_orders_in_shipment(shipment_id)
    //     for x in packed_orders:
    //         if x not in orders:
    //             order = self.get_order(x)
    //             order["shipment_id"] = -1
    //             order["order_status"] = "Scheduled"
    //             self.update_order(x, order)
    //     for x in orders:
    //         order = self.get_order(x)
    //         order["shipment_id"] = shipment_id
    //         order["order_status"] = "Packed"
    //         self.update_order(x, order)

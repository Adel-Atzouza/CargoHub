using CargoHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoHub.Models;
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
    }
}

    // def get_items_in_order(self, order_id):
    //     for x in self.data:
    //         if x["id"] == order_id:
    //             return x["items"]
    //     return None

    // def get_orders_in_shipment(self, shipment_id):
    //     result = []
    //     for x in self.data:
    //         if x["shipment_id"] == shipment_id:
    //             result.append(x["id"])
    //     return result
        // def get_orders_for_client(self, client_id):
        // result = []
        // for x in self.data:
        //     if x["ship_to"] == client_id or x["bill_to"] == client_id:
        //         result.append(x)


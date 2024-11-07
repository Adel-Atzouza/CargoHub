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
            if(order.Id != 0 || order.CreatedAt != default || order.UpdatedAt != default)
            {
                return "the id, created at and updated at should not be provided";
            }

        }
    }
}
    // def add_order(self, order):
    //     order["created_at"] = self.get_timestamp()
    //     order["updated_at"] = self.get_timestamp()
    //     self.data.append(order)

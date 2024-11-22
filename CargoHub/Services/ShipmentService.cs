using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class ShipmentService
    {
        private readonly AppDbContext _context;

        public ShipmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Shipment?> GetShipmentByIdAsync(int id)
            {
                var shipment = await _context.Shipments
                    .Include(s => s.orders)            // Load Orders related to the Shipment
                    .ThenInclude(o => o.OrderItems)    // Load OrderItems for each Order
                    .FirstOrDefaultAsync(s => s.Id == id);

                return shipment;
            }


    }
}

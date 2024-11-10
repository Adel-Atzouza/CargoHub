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

        // Get all shipments
        public async Task<List<Shipment>> GetShipments()
        {
            return await _context.Shipments.ToListAsync();
        }
        public async Task<Shipment?> GetShipmentID(int shipmentId)
        {
            return await _context.Shipments
                .Include(s => s.Items) // Include related items in the shipment
                .FirstOrDefaultAsync(s => s.Id == shipmentId);
        }

        public async Task<List<Shipment>> GetShipmentsByShipmentDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Shipments
                .Where(shipment => shipment.ShipmentDate >= startDate && shipment.ShipmentDate <= endDate)
                .OrderBy(shipment => shipment.ShipmentDate)
                .ToListAsync();
        }



        public async Task<List<Item>?> GetItemsInShipment(int shipmentId)
        {
            var shipment = await GetShipmentID(shipmentId);
            return shipment?.Items;
        }
        public async Task<string> AddShipment(Shipment shipment)
        {
            shipment.CreatedAt = DateTime.UtcNow;
            shipment.UpdatedAt = DateTime.UtcNow;
            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();
            return "Shipment added successfully.";
        }

        public async Task<string> UpdateShipment(int shipmentId, Shipment updatedShipment)
        {
            var existingShipment = await _context.Shipments.FindAsync(shipmentId);
            if (existingShipment == null)
            {
                return "Error: Shipment not found.";
            }

            updatedShipment.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingShipment).CurrentValues.SetValues(updatedShipment);
            await _context.SaveChangesAsync();
            return "Shipment updated successfully.";
        }
        public async Task<string> RemoveShipment(int shipmentId)
        {
            var shipment = await _context.Shipments.FindAsync(shipmentId);
            if (shipment == null)
            {
                return "Error: Shipment not found.";
            }

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return "Shipment removed successfully.";
        }
    }
}

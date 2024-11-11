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

        // Get all shipments with related Orders and OrderItems (including Items)
        public async Task<List<Shipment>> GetShipmentswithdetails()
        {
            return await _context.Shipments
                .Include(s => s.orders) // Include related orders in the shipment
                .ThenInclude(o => o.OrderItems) // Include order items in the order
                .ThenInclude(oi => oi.Item) // Include the related item in the order item
                .ToListAsync();
        }

        // Get shipment by ID with related Orders and OrderItems (including Items)
        public async Task<Shipment?> GetShipmentID(int shipmentId)
        {
            return await _context.Shipments
                .Include(s => s.orders) // Include related orders in the shipment
                .ThenInclude(o => o.OrderItems) // Include order items in the order
                .ThenInclude(oi => oi.Item) // Include the related item in the order item
                .FirstOrDefaultAsync(s => s.Id == shipmentId);
        }

        // Get shipments by date range
        public async Task<List<Shipment>> GetShipmentsByShipmentDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Shipments
                .Where(shipment => shipment.ShipmentDate >= startDate && shipment.ShipmentDate <= endDate)
                .OrderBy(shipment => shipment.ShipmentDate)
                .ToListAsync();
        }

        // Add a new shipment with a list of Order IDs
        public async Task<string> AddShipment(ShipmentRequest shipmentRequest)
        {
            var shipment = new Shipment
            {
                SourceId = shipmentRequest.SourceId,
                Orderdate = shipmentRequest.Orderdate,
                RequestDate = shipmentRequest.RequestDate,
                ShipmentDate = shipmentRequest.ShipmentDate,
                ShipmentType = shipmentRequest.ShipmentType,
                ShipmentStatus = shipmentRequest.ShipmentStatus,
                Notes = shipmentRequest.Notes,
                CarrierCode = shipmentRequest.CarrierCode,
                CarrierDescription = shipmentRequest.CarrierDescription,
                ServiceCode = shipmentRequest.ServiceCode,
                PaymentType = shipmentRequest.PaymentType,
                TransferMode = shipmentRequest.TransferMode,
                TotalPackageCount = shipmentRequest.TotalPackageCount,
                TotalPackageWeight = shipmentRequest.TotalPackageWeight,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                orders = new List<Order>() // Initialize the Orders list
            };

            // Add the orders associated with the shipment
            foreach (var orderId in shipmentRequest.OrderIds)
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    order.ShipmentId = shipment.Id; // Link the Order to the Shipment
                    shipment.orders.Add(order); // Add the Order to the Shipment's Orders list
                }
            }

            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();
            return "Shipment added successfully.";
        }

        // Update an existing shipment with new details
        public async Task<string> UpdateShipment(int shipmentId, ShipmentRequest shipmentRequest)
        {
            var existingShipment = await _context.Shipments.FindAsync(shipmentId);
            if (existingShipment == null)
            {
                return "Error: Shipment not found.";
            }

            existingShipment.SourceId = shipmentRequest.SourceId;
            existingShipment.Orderdate = shipmentRequest.Orderdate;
            existingShipment.RequestDate = shipmentRequest.RequestDate;
            existingShipment.ShipmentDate = shipmentRequest.ShipmentDate;
            existingShipment.ShipmentType = shipmentRequest.ShipmentType;
            existingShipment.ShipmentStatus = shipmentRequest.ShipmentStatus;
            existingShipment.Notes = shipmentRequest.Notes;
            existingShipment.CarrierCode = shipmentRequest.CarrierCode;
            existingShipment.CarrierDescription = shipmentRequest.CarrierDescription;
            existingShipment.ServiceCode = shipmentRequest.ServiceCode;
            existingShipment.PaymentType = shipmentRequest.PaymentType;
            existingShipment.TransferMode = shipmentRequest.TransferMode;
            existingShipment.TotalPackageCount = shipmentRequest.TotalPackageCount;
            existingShipment.TotalPackageWeight = shipmentRequest.TotalPackageWeight;
            existingShipment.UpdatedAt = DateTime.UtcNow;

            // Update the Orders for the Shipment
            existingShipment.orders.Clear(); // Clear the existing orders
            foreach (var orderId in shipmentRequest.OrderIds)
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    order.ShipmentId = existingShipment.Id;
                    existingShipment.orders.Add(order);
                }
            }

            await _context.SaveChangesAsync();
            return "Shipment updated successfully.";
        }

        // Remove a shipment by its ID
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

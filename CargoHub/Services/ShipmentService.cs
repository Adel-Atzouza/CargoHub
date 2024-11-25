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

        // Constructor to inject the AppDbContext dependency
        public ShipmentService(AppDbContext context)
        {
            _context = context;
        }

        // Method to get a specific shipment by its ID, including related orders and items
        public async Task<object?> GetShipmentByIdAsync(int shipmentId)
        {
            // Query the database to retrieve the shipment by its ID
            var shipment = await _context.Shipments
                .Include(s => s.orders) // Include related orders
                .ThenInclude(o => o.OrderItems) // Include order items for each order
                .ThenInclude(oi => oi.Item) // Include item details for each order item
                .FirstOrDefaultAsync(s => s.Id == shipmentId); // Find the shipment by its ID

            // If the shipment is not found, return null
            if (shipment == null)
            {
                return null;
            }

            // Map the shipment to a custom object with the desired structure
            var result = new
            {
                shipment.Id, // Shipment ID
                shipment.SourceId, // Source ID
                shipment.ShipmentDate, // Date of shipment
                shipment.ShipmentType, // Type of shipment (e.g., Air, Sea)
                shipment.ShipmentStatus, // Current status of the shipment
                shipment.Notes, // Additional notes for the shipment
                shipment.CarrierCode, // Carrier code (e.g., DHL, FedEx)
                shipment.CarrierDescription, // Description of the carrier
                shipment.ServiceCode, // Service code for the shipment
                shipment.PaymentType, // Payment type (e.g., Prepaid, Collect)
                shipment.TransferMode, // Mode of transfer (e.g., Road, Rail)
                shipment.TotalPackageCount, // Total number of packages in the shipment
                shipment.TotalPackageWeight, // Total weight of the packages
                shipment.CreatedAt, // Timestamp for when the shipment was created
                shipment.UpdatedAt, // Timestamp for the last update to the shipment
                Orders = shipment.orders.Select(o => new
                {
                    o.Id, // Order ID
                    o.OrderDate, // Date the order was placed
                    o.RequestDate, // Requested delivery date
                    o.Reference, // Order reference number
                    o.OrderStatus, // Status of the order (e.g., Pending, Delivered)
                    Items = o.OrderItems.Select(oi => new
                    {
                        ItemId = oi.Item.Uid, // Unique identifier for the item
                        oi.Amount // Quantity of the item in the order
                    })
                })
            };

            // Return the mapped shipment object
            return result;
        }

        public async Task<bool> DeleteShipment(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);

            if (shipment == null)
            {
                return false;
            }
            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

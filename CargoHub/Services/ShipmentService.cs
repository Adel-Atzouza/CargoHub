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

        // Constructor om de databasecontext te injecteren
        public ShipmentService(AppDbContext context)
        {
            _context = context;
        }

        // Haal alle zendingen op uit de database, inclusief orders en items
        public async Task<List<ShipmentDTO>> GetAllShipmentsWithItems()
        {
            var shipments = await _context.Shipments
                .Include(s => s.orders) // Haal de orders van de zending op
                .ThenInclude(o => o.OrderItems) // Haal de orderitems voor elke order op
                .ThenInclude(oi => oi.Item) // Haal de item details voor elk orderitem op
                .ToListAsync();

            // Map de zendingen naar ShipmentDTO objecten
            var result = shipments.Select(shipment => new ShipmentDTO
            {
                Id = shipment.Id,
                ShipmentDate = shipment.ShipmentDate,
                ShipmentType = shipment.ShipmentType,
                ShipmentStatus = shipment.ShipmentStatus,
                Notes = shipment.Notes,
                CarrierCode = shipment.CarrierCode,
                CarrierDescription = shipment.CarrierDescription,
                ServiceCode = shipment.ServiceCode,
                PaymentType = shipment.PaymentType,
                TransferMode = shipment.TransferMode,
                TotalPackageCount = shipment.TotalPackageCount,
                TotalPackageWeight = shipment.TotalPackageWeight,
                CreatedAt = shipment.CreatedAt,
                UpdatedAt = shipment.UpdatedAt,
                Orders = shipment.orders.Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    RequestDate = o.RequestDate,
                    Reference = o.Reference,
                    OrderStatus = o.OrderStatus,
                    Items = o.OrderItems.Select(oi => new ItemDTO
                    {
                        ItemId = oi.Item.Uid,
                        Amount = oi.Amount
                    }).ToList()
                }).ToList()
            }).ToList();

            return result; // Retourneer de lijst van ShipmentDTO's
        }

        // Haal een specifieke zending op, inclusief orders en hun details
        public async Task<ShipmentDTO?> GetShipmentByIdWithOrderDetails(int shipmentId)
        {
            // Haal de zending op met orders en items, gekoppeld via de relaties in de database
            var shipment = await _context.Shipments
                .Include(s => s.orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
            {
                return null; // Als de zending niet bestaat, retourneer null
            }

            // Map de zending naar een ShipmentDTO object
            var result = new ShipmentDTO
            {
                Id = shipment.Id,
                ShipmentDate = shipment.ShipmentDate,
                ShipmentType = shipment.ShipmentType,
                ShipmentStatus = shipment.ShipmentStatus,
                Notes = shipment.Notes,
                CarrierCode = shipment.CarrierCode,
                CarrierDescription = shipment.CarrierDescription,
                ServiceCode = shipment.ServiceCode,
                PaymentType = shipment.PaymentType,
                TransferMode = shipment.TransferMode,
                TotalPackageCount = shipment.TotalPackageCount,
                TotalPackageWeight = shipment.TotalPackageWeight,
                CreatedAt = shipment.CreatedAt,
                UpdatedAt = shipment.UpdatedAt,
                Orders = shipment.orders.Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    RequestDate = o.RequestDate,
                    Reference = o.Reference,
                    OrderStatus = o.OrderStatus,
                    Items = o.OrderItems.Select(oi => new ItemDTO
                    {
                        ItemId = oi.Item.Uid,
                        Amount = oi.Amount
                    }).ToList()
                }).ToList()
            };

            return result; // Retourneer de ShipmentDTO met orderdetails
        }

        // Haal alleen de items op voor een specifieke zending
        public async Task<object?> GetShipmentItems(int shipmentId)
        {
            // Haal de zending op met de orders en de items van die orders
            var shipment = await _context.Shipments
                .Include(s => s.orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
            {
                return null; // Als de zending niet gevonden is, retourneer null
            }

            // Haal de items per zending op en groepeer ze op item ID
            var items = shipment.orders
                .SelectMany(o => o.OrderItems) // Haal alle items van de orders
                .GroupBy(oi => oi.Item.Uid) // Groepeer de items op hun unieke ID
                .Select(group => new
                {
                    itemId = group.Key, // Het item ID
                    amount = group.Sum(oi => oi.Amount) // Sommeer de hoeveelheden van elk item
                }).ToList();

            // Retourneer de zending met de samengevoegde items
            var result = new
            {
                shipment.Id,
                shipment.ShipmentDate,
                shipment.ShipmentType,
                shipment.ShipmentStatus,
                shipment.Notes,
                shipment.CarrierCode,
                shipment.CarrierDescription,
                shipment.ServiceCode,
                shipment.PaymentType,
                shipment.TransferMode,
                shipment.TotalPackageCount,
                shipment.TotalPackageWeight,
                shipment.CreatedAt,
                shipment.UpdatedAt,
                items // Samengevoegde items voor deze zending
            };

            return result; // Retourneer het object met de items
        }

        // Maak een nieuwe zending aan
        public async Task<Shipment> CreateShipment(Shipment shipment)
        {
            try
            {
                _context.Shipments.Add(shipment); // Voeg de nieuwe zending toe
                await _context.SaveChangesAsync(); // Sla de zending op in de database
                return shipment; // Retourneer de aangemaakte zending
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating shipment: {ex.Message}"); // Fout bij aanmaken van zending
                throw;
            }
        }

        // Ken orders toe aan een specifieke zending
        public async Task<bool> AssignOrdersToShipment(int shipmentId, List<int> orderIds)
        {
            // Haal de zending op die we willen bijwerken
            var shipment = await _context.Shipments
                .Include(s => s.orders) // Laad de bestaande orders in de zending
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
            {
                return false; // Zending bestaat niet
            }

            // Zoek de orders op basis van de gegeven IDs
            var orders = await _context.Orders
                .Where(o => orderIds.Contains(o.Id))
                .ToListAsync();

            if (orders.Count != orderIds.Count)
            {
                return false; // Stop als er ontbrekende orders zijn
            }

            // Koppel de gevonden orders aan de zending
            foreach (var order in orders)
            {
                order.ShipmentId = shipmentId;
            }

            await _context.SaveChangesAsync(); // Sla de wijzigingen op
            return true;
        }

        // Update welke orders aan een zending gekoppeld zijn
        public async Task<bool> UpdateOrdersInShipment(int shipmentId, List<int> orderIds)
        {
            var packedOrders = await _context.Orders
                .Where(o => o.ShipmentId == shipmentId)
                .ToListAsync();

            foreach (var order in packedOrders)
            {
                if (!orderIds.Contains(order.Id))
                {
                    order.ShipmentId = null; // Haal de koppeling met de zending weg
                    order.OrderStatus = "Scheduled"; // Update de status
                }
            }

            // Werk de gekoppelde orders bij
            foreach (var orderId in orderIds)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order != null)
                {
                    order.ShipmentId = shipmentId; // Koppel de order aan de zending
                    order.OrderStatus = "Packed"; // Update de status
                }
            }

            await _context.SaveChangesAsync(); // Sla de wijzigingen op
            return true;
        }

        // Verwijder een zending
        public async Task<bool> DeleteShipment(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return false; // Zending bestaat niet
            }

            _context.Shipments.Remove(shipment); // Verwijder de zending
            await _context.SaveChangesAsync(); // Sla de wijzigingen op
            return true; // Retourneer succes
        }
    }
}

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

        public async Task<List<dynamic>> GetAllShipmentsWithItems()
        {
             // Haal alle zendingen op uit de database, inclusief hun gerelateerde orders en items
            var shipments = await _context.Shipments
                .Include(s => s.orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();

            var result = shipments.Select(shipment => new
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
                items = shipment.orders
                    .SelectMany(o => o.OrderItems)
                    .GroupBy(oi => oi.Item.Uid)
                    .Select(group => new
                    {
                        itemId = group.Key,
                        amount = group.Sum(oi => oi.Amount)
                    }).ToList()
            }).ToList();
            // Retourneer het resultaat als een lijst van dynamische objecten
            return result.Cast<dynamic>().ToList();
        }



        // Haal een specifieke zending op, inclusief orders en hun details
        public async Task<object?> GetShipmentByIdWithOrderDetails(int shipmentId)
        {
            // Zoek de zending op in de database, inclusief de gerelateerde orders en items
            var shipment = await _context.Shipments
                .Include(s => s.orders) // Voeg orders toe
                .ThenInclude(o => o.OrderItems) // Voeg items van de orders toe
                .ThenInclude(oi => oi.Item) // Voeg details van elk item toe
                .FirstOrDefaultAsync(s => s.Id == shipmentId); // Zoek de zending met het opgegeven ID

            if (shipment == null)
            {
                return null; // Als de zending niet bestaat, geef null terug
            }

            // Bouw een object met alleen de velden die je wilt teruggeven
            var result = new
            {
                shipment.Id,
                shipment.SourceId,
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
                Orders = shipment.orders.Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.RequestDate,
                    o.Reference,
                    o.OrderStatus,
                    Items = o.OrderItems.Select(oi => new
                    {
                        ItemId = oi.Item.Uid,
                        oi.Amount
                    })
                })
            };

            return result; // Retourneer het aangepaste object
        }

        // Ken orders toe aan een specifieke zending
        public async Task<bool> AssignOrdersToShipment(int shipmentId, List<int> orderIds)
        {
            var shipment = await _context.Shipments
                .Include(s => s.orders) // Laad bestaande orders in de zending
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
            {
                return false; // Als de zending niet bestaat, stop
            }

            var orders = await _context.Orders
                .Where(o => orderIds.Contains(o.Id)) // Zoek orders op basis van de gegeven IDs
                .ToListAsync();

            if (orders.Count != orderIds.Count)
            {
                return false; // Stop als er ontbrekende orders zijn
            }

            foreach (var order in orders)
            {
                order.ShipmentId = shipmentId; // Koppel de order aan de zending
            }

            await _context.SaveChangesAsync(); // Sla de wijzigingen op
            return true;
        }

        // Haal alleen de items uit een zending (zonder duplicaten en met samengevoegde hoeveelheden)
        public async Task<object?> GetShipmentItems(int shipmentId)
        {
            var shipment = await _context.Shipments
                .Include(s => s.orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
            {
                return null; // Zending niet gevonden
            }

            var items = shipment.orders
                .SelectMany(o => o.OrderItems) // Pak alle items van alle orders
                .GroupBy(oi => oi.Item.Uid) // Groepeer op unieke item-ID
                .Select(group => new
                {
                    itemId = group.Key, // Unieke item-ID
                    amount = group.Sum(oi => oi.Amount) // Tel de hoeveelheden bij elkaar op
                }).ToList();

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
                items
            };

            return result; // Retourneer de zending met items
        }

        // Maak een nieuwe zending aan
        public async Task<Shipment> CreateShipment(Shipment shipment)
        {
            try
            {
                _context.Shipments.Add(shipment); // Voeg de zending toe
                await _context.SaveChangesAsync(); // Sla de zending op
                return shipment; // Retourneer de gemaakte zending
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating shipment: {ex.Message}");
                throw; // Gooi de fout opnieuw voor debugging
            }
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

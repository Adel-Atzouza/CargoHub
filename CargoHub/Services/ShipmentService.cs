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

        // Haal alle zendingen op met gekoppelde orders en items
        public async Task<List<ShipmentDTO>> GetAllShipmentsWithorderdetailsItems()
        {
            var shipments = await _context.Shipments
                .Include(s => s.orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();

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

            return result;
        }

        // Haal een specifieke zending op met gekoppelde orders en items
        public async Task<ShipmentDTO?> GetShipmentByIdWithOrderDetails(int shipmentId)
        {
            var shipment = await _context.Shipments
                .Include(s => s.orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
            {
                return null;
            }

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

            return result;
        }

        // Haal alle zendingen op met alleen de items van de orders
        public async Task<List<ShipmentDTO>> GetAllShipmentsWithItems()
        {
            var shipments = await _context.Shipments
                .Include(s => s.orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();

            var shipmentDTOs = shipments.Select(shipment => new ShipmentDTO
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
                Orders = shipment.orders.Select(order => new OrderDTO
                {
                    Items = order.OrderItems.Select(orderItem => new ItemDTO
                    {
                        ItemId = orderItem.Item.Uid,
                        Amount = orderItem.Amount
                    }).ToList()
                }).ToList()
            }).ToList();

            return shipmentDTOs;
        }

        // Haal alleen de items op voor een specifieke zending
        public async Task<object?> GetShipmentItems(int shipmentId)
        {
            var shipment = await _context.Shipments
                .Include(s => s.orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
            {
                return null;
            }

            var items = shipment.orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Item.Uid)
                .Select(group => new
                {
                    itemId = group.Key,
                    amount = group.Sum(oi => oi.Amount)
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

            return result;
        }

        // Maak een nieuwe zending aan
        public async Task<Shipment> CreateShipment(Shipment shipment)
        {
            try
            {
                _context.Shipments.Add(shipment);
                await _context.SaveChangesAsync();
                return shipment;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het aanmaken van zending: {ex.Message}");
                throw;
            }
        }

        // Update de velden van een zending
        public async Task<string> UpdateShipmentFields(int shipmentId, ShipmentDTO updatedShipmentDto)
        {
            var existingShipment = await _context.Shipments
                .Include(s => s.orders)
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (existingShipment == null)
            {
                return $"Zending met ID {shipmentId} is niet gevonden.";
            }

            if (!string.IsNullOrEmpty(updatedShipmentDto.ShipmentStatus) &&
                updatedShipmentDto.ShipmentStatus.Equals("transit", StringComparison.OrdinalIgnoreCase) &&
                !existingShipment.ShipmentStatus.Equals("transit", StringComparison.OrdinalIgnoreCase))
            {
                existingShipment.ShipmentDate = DateTime.UtcNow;

                foreach (var order in existingShipment.orders)
                {
                    order.OrderStatus = "shipped";
                }
            }

            if (!string.IsNullOrEmpty(updatedShipmentDto.ShipmentStatus) &&
                updatedShipmentDto.ShipmentStatus.Equals("delivered", StringComparison.OrdinalIgnoreCase) &&
                !existingShipment.ShipmentStatus.Equals("delivered", StringComparison.OrdinalIgnoreCase))
            {
                existingShipment.Orderdate = DateTime.UtcNow;
            }

            if (!string.IsNullOrEmpty(updatedShipmentDto.ShipmentStatus) &&
                updatedShipmentDto.ShipmentStatus.Equals("pending", StringComparison.OrdinalIgnoreCase) &&
                (existingShipment.ShipmentStatus.Equals("transit", StringComparison.OrdinalIgnoreCase) ||
                existingShipment.ShipmentStatus.Equals("delivered", StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Kan de status van de zending niet wijzigen naar pending vanaf transit of delivered.");
            }

            existingShipment.ShipmentType = updatedShipmentDto.ShipmentType;
            existingShipment.ShipmentStatus = updatedShipmentDto.ShipmentStatus;
            existingShipment.Notes = updatedShipmentDto.Notes;
            existingShipment.CarrierCode = updatedShipmentDto.CarrierCode;
            existingShipment.CarrierDescription = updatedShipmentDto.CarrierDescription;
            existingShipment.ServiceCode = updatedShipmentDto.ServiceCode;
            existingShipment.PaymentType = updatedShipmentDto.PaymentType;
            existingShipment.TransferMode = updatedShipmentDto.TransferMode;
            existingShipment.TotalPackageCount = updatedShipmentDto.TotalPackageCount;
            existingShipment.TotalPackageWeight = updatedShipmentDto.TotalPackageWeight;
            existingShipment.UpdatedAt = DateTime.UtcNow;

            _context.Shipments.Update(existingShipment);
            await _context.SaveChangesAsync();
            return $"Zending met ID {shipmentId} is succesvol bijgewerkt.";
        }

        // Koppel orders aan een zending
        public async Task<bool> AssignOrdersToShipment(int shipmentId, List<int> orderIds)
        {
            var shipment = await _context.Shipments
                .Include(s => s.orders)
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
            {
                return false;
            }

            var orders = await _context.Orders
                .Where(o => orderIds.Contains(o.Id))
                .ToListAsync();

            if (orders.Count != orderIds.Count)
            {
                return false;
            }

            foreach (var order in orders)
            {
                order.ShipmentId = shipmentId;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Werk orders bij die aan een zending gekoppeld zijn
        public async Task<bool> UpdateOrdersInShipment(int shipmentId, List<int> orderIds)
        {
            var packedOrders = await _context.Orders
                .Where(o => o.ShipmentId == shipmentId)
                .ToListAsync();

            foreach (var order in packedOrders)
            {
                if (!orderIds.Contains(order.Id))
                {
                    order.ShipmentId = null;
                    order.OrderStatus = "Scheduled";
                }
            }

            foreach (var orderId in orderIds)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order != null)
                {
                    order.ShipmentId = shipmentId;
                    order.OrderStatus = "Packed";
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Verwijder een zending
        public async Task<bool> DeleteShipment(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return false;
            }

            var ordersid = await _context.Orders.Where(o => o.ShipmentId == id).ToListAsync();
            foreach(var order in ordersid)
            {
                order.ShipmentId = null;
                order.OrderStatus = "Scheduled";
            }
            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

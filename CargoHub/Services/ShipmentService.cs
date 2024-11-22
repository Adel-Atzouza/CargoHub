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

    public async Task<ShipmentDTO?> GetShipmentByIdAsync(int shipmentId)
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

        // Map to ShipmentDTO
        var shipmentDTO = new ShipmentDTO
        {
            Id = shipment.Id,
            ShipmentDate = shipment.ShipmentDate,
            ShipmentType = shipment.ShipmentType,
            ShipmentStatus = shipment.ShipmentStatus,
            Orders = shipment.orders.Select(o => new OrderWithItemsDTO
            {
                Id = o.Id,
                Reference = o.Reference,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new ItemDTO
                {
                    ItemId = oi.Item.Uid,
                    Amount = oi.Amount
                }).ToList()
            }).ToList()
        };

        return shipmentDTO;
    }
}
}

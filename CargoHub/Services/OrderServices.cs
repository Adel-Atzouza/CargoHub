using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CargoHub.Models;

namespace CargoHub.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        // Constructor om de database context te injecteren
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // Haal een specifieke order op met alle items erbij
        public async Task<OrderWithItemsDTO> GetOrderWithItems(int id)
        {
            // Zoek de order op basis van ID
            var order = await _context.Orders
                .Where(o => o.Id == id) // Filter op het ID van de order
                .Select(o => new OrderWithItemsDTO
                {
                    Id = o.Id,
                    SourceId = o.SourceId,
                    OrderDate = o.OrderDate,
                    RequestDate = o.RequestDate,
                    Reference = o.Reference,
                    ReferenceExtra = o.ExtrReference,
                    OrderStatus = o.OrderStatus,
                    Notes = o.Notes,
                    ShippingNotes = o.ShippingNotes,
                    PickingNotes = o.PickingNotes,
                    WarehouseId = o.WarehouseId,
                    ShipTo = o.ShipTo,
                    BillTo = o.BillTo,
                    ShipmentId = o.ShipmentId,
                    TotalAmount = o.TotalAmount,
                    TotalDiscount = o.TotalDiscount,
                    TotalTax = o.TotalTax,
                    TotalSurcharge = o.TotalSurcharge,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    Items = o.OrderItems.Select(oi => new ItemDTO
                    {
                        ItemId = oi.Item.Uid, // Het unieke ID van het item
                        Amount = oi.Amount // Hoeveelheid van het item
                    }).ToList()
                })
                .FirstOrDefaultAsync(); // Pak de eerste die voldoet, of null als er niks is

            return order; // Retourneer de order met details
        }

        // Haal alle orders op voor een specifieke klant
        public async Task<List<Order>> GetOrdersCLient(int id)
        {
            // Zoek orders waar de klant als ontvanger of betaler wordt genoemd
            return await _context.Orders
                .Where(c => c.BillTo == id || c.ShipTo == id)
                .ToListAsync(); // Geef een lijst terug met alle matches
        }

        // Haal alle orders met hun items erbij
        public async Task<List<OrderWithItemsDTO>> GetAllOrdersWithItems()
        {
            return await _context.Orders
                .Select(o => new OrderWithItemsDTO
                {
                    Id = o.Id,
                    SourceId = o.SourceId,
                    OrderDate = o.OrderDate,
                    RequestDate = o.RequestDate,
                    Reference = o.Reference,
                    ReferenceExtra = o.ExtrReference,
                    OrderStatus = o.OrderStatus,
                    Notes = o.Notes,
                    ShippingNotes = o.ShippingNotes,
                    PickingNotes = o.PickingNotes,
                    WarehouseId = o.WarehouseId,
                    ShipTo = o.ShipTo,
                    BillTo = o.BillTo,
                    ShipmentId = o.ShipmentId,
                    TotalAmount = o.TotalAmount,
                    TotalDiscount = o.TotalDiscount,
                    TotalTax = o.TotalTax,
                    TotalSurcharge = o.TotalSurcharge,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    Items = o.OrderItems.Select(oi => new ItemDTO
                    {
                        ItemId = oi.Item.Uid, // Het unieke ID van het item
                        Amount = oi.Amount // Hoeveelheid van het item
                    }).ToList()
                })
                .ToListAsync(); // Retourneer een lijst met alle orders en items
        }

        // Maak een nieuwe order aan met items
        public async Task<Order> CreateOrder(Order order, List<ItemDTO> itemDTOs)
        {
            try
            {
                // Voeg de order toe aan de databasecontext (nog niet opgeslagen in de database)
                _context.Orders.Add(order);

                // Controleer en verwerk de items
                foreach (var itemDto in itemDTOs)
                {
                    // Zoek het item op in de database via het unieke ID
                    var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);
                    if (item != null)
                    {
                        // Controleer of er genoeg voorraad is
                        if (item.UnitOrderQuantity < itemDto.Amount)
                        {
                            throw new Exception($"Niet genoeg voorraad voor item met Uid: {itemDto.ItemId}. Beschikbaar: {item.UnitOrderQuantity}, gevraagd: {itemDto.Amount}");
                        }

                        // Verminder de voorraad (alleen in de context, nog niet opgeslagen)
                        item.UnitOrderQuantity -= itemDto.Amount;

                        // Maak een OrderItem aan en voeg toe aan de context
                        var orderItem = new OrderItem
                        {
                            Order = order, // Koppel het direct aan de order in de context
                            ItemUid = item.Uid,
                            Amount = itemDto.Amount
                        };
                        _context.OrderItems.Add(orderItem);
                    }
                    else
                    {
                        throw new Exception($"Item met Uid {itemDto.ItemId} bestaat niet.");
                    }
                }

                // Sla alles in één keer op nadat alle checks geslaagd zijn
                await _context.SaveChangesAsync();

                return order; // Retourneer de aangemaakte order
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het aanmaken van een order: {ex.Message}");

                // Gooi de fout opnieuw, zodat deze hogerop kan worden afgehandeld
                throw;
            }
        }

    public async Task<string> UpdateItemsInOrder(int orderId, List<ItemDTO> orderItems)
    {
        // Stap 1: Controleer of de opgegeven items bestaan
        foreach (var item in orderItems)
        {
            if (!await ItemExist(item.ItemId))
            {
                return $"Item met ID {item.ItemId} is niet gevonden.";
            }
        }

        // Stap 2: Haal de bestaande order op
        var updateOrderDto = await GetOrderWithItems(orderId);
        if (updateOrderDto == null)
        {
            return $"Order met ID {orderId} is niet gevonden.";
        }

        // Stap 3: Map de DTO naar een Order-entiteit
        var updateOrder = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);
        if (updateOrder == null)
        {
            return $"Order met ID {orderId} is niet gevonden in de database.";
        }

        // Werk de bestaande items bij
        foreach (var currentItem in updateOrder.OrderItems)
        {
            var matchingItem = orderItems.FirstOrDefault(i => i.ItemId == currentItem.ItemUid);
            if (matchingItem != null)
            {
                var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == currentItem.ItemUid);
                if (inventoryItem != null)
                {
                    inventoryItem.TotalAllocated += matchingItem.Amount - currentItem.Amount;
                }
                currentItem.Amount = matchingItem.Amount; // Update de hoeveelheid
            }
        }

        // Voeg nieuwe items toe aan de order
        foreach (var newItem in orderItems)
        {
            var existingItem = updateOrder.OrderItems.FirstOrDefault(i => i.ItemUid == newItem.ItemId);
            if (existingItem == null)
            {
                var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == newItem.ItemId);
                if (inventoryItem != null)
                {
                    if (inventoryItem.TotalAvailable < newItem.Amount)
                    {
                        return $"Niet genoeg voorraad voor item met ID {newItem.ItemId}. Beschikbaar: {inventoryItem.TotalAvailable}, gevraagd: {newItem.Amount}";
                    }

                    var orderItem = new OrderItem
                    {
                        ItemUid = newItem.ItemId,
                        Amount = newItem.Amount,
                        OrderId = orderId
                    };

                    _context.OrderItems.Add(orderItem); // Registreer het item in de database
                    updateOrder.OrderItems.Add(orderItem); // Voeg toe aan de lijst in de order
                    inventoryItem.TotalAllocated += newItem.Amount; // Update voorraad
                }
            }
        }

        // Stap 5: Sla de wijzigingen op
        _context.Orders.Update(updateOrder);
        await _context.SaveChangesAsync();

        return $"Order met ID {orderId} is succesvol bijgewerkt.";
    }

        // Check of een item bestaat op basis van het unieke ID
        public async Task<bool> ItemExist(string itemid)
        {
            return await _context.Items.AnyAsync(i => i.Uid == itemid); // True of False
        }

        // Verwijder een order op basis van ID
        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Item)
            .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return false; // Bestaat niet? Geef False terug

            foreach (var orderItem in order.OrderItems)
            {
                var item = orderItem.Item;
                if (item != null)
                {
                    item.UnitOrderQuantity += orderItem.Amount;
                }
            }
            _context.Orders.Remove(order); // Verwijder de order
            await _context.SaveChangesAsync(); // Opslaan in de database
            return true; // Geef True terug om succes aan te geven
        }
    }
}

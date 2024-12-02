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

        public async Task<string> UpdateitemsInOrder(int orderid, List<ItemDTO> orderitems )
        {
            foreach( var x in orderitems)
            {
                if(!await ItemExist(x.ItemId))
                {
                    return $"item met id {x.ItemId} is not found";
                }
            }
            var Updateorder = await GetOrderWithItems(orderid);
            if(Updateorder == null ){
                return $"order with {orderid} not found";
            }

            var currentitems = Updateorder.Items;

            //STAP 1 het updaten van bestaande items in the huidige order
            foreach (var x in currentitems)
            {
                foreach(var y in orderitems)
                {

                    if (x.ItemId == y.ItemId)
                    {
                        var inventoryitem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == x.ItemId);//zoek de item op 
                        if(inventoryitem != null){
                            inventoryitem.TotalAllocated += y.Amount - x.Amount;
                        }
                        x.Amount = y.Amount;
                        break;
                    }
                }
            }

            //STAP 2 het toevoegen van nieuwe items aan de huidige order
            foreach (var y in orderitems)
            {
                var existingItem = currentitems.FirstOrDefault(i => i.ItemId == y.ItemId);
                if (existingItem != null)
                {
                    var inventoryitem = await _context.Items.FirstOrDefaultAsync(i => i.Uid == y.ItemId);//zoek de item op
                    if(inventoryitem != null){
                        {
                            if (inventoryitem.UnitOrderQuantity < y.Amount)
                            {
                                return $"Niet genoeg voorraad voor item met Uid: {inventoryitem.Uid}. Beschikbaar: {inventoryitem.UnitOrderQuantity}, gevraagd: {y.Amount}";
                            }
                        }
                    }
                }
            }

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

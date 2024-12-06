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
                // Voeg de order toe aan de database
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Opslaan zodat het ID wordt gegenereerd

                // Voeg items toe aan de order
                foreach (var itemDto in itemDTOs)
                {
                    // Zoek het item op in de database via het unieke ID
                    var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);
                    if (item != null)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id, // Koppel aan het gegenereerde order-ID
                            ItemUid = item.Uid, // Het unieke ID van het item
                            Amount = itemDto.Amount // Hoeveelheid van het item
                        };
                        _context.OrderItems.Add(orderItem); // Voeg de relatie toe
                    }
                    else
                    {
                        throw new Exception($"Item met Uid {itemDto.ItemId} bestaat niet.");
                    }
                }

                await _context.SaveChangesAsync(); // Opslaan in de database
                return order; // Retourneer de aangemaakte order
            }
            catch (Exception ex)
            {
                // Log de fout en gooi hem opnieuw voor verder gebruik
                Console.WriteLine($"Fout bij het aanmaken van een order: {ex.Message}");
                throw;
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
            var order = await _context.Orders.FindAsync(id); // Zoek de order op
            if (order == null) return false; // Bestaat niet? Geef False terug

            _context.Orders.Remove(order); // Verwijder de order
            await _context.SaveChangesAsync(); // Opslaan in de database
            return true; // Geef True terug om succes aan te geven
        }
    }
}
// public async Task<string> UpdateitemsInOrder(int orderid, List<ItemDTO> orderitems )
// {
//     foreach( var x in orderitems)
//     {
//         if(!await ItemExist(x.ItemId))
//         {
//             return $"item met id {x.ItemId} is not found";
//         }
//     }
//     var Updateorder = await GetOrderWithItems(orderid);
//     if(Updateorder == null ){
//         return $"order with {orderid} not found";
//     }

//     var currentitems = Updateorder.Items;

//     //STAP 1 het updaten van bestaande items in the huidige order
//     foreach (var x in currentitems)
//     {
//         foreach(var y in orderitems)
//         {

//             if (x.ItemId == y.ItemId)
//             {
//                 var inventoryitem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == x.ItemId);
//                 if(inventoryitem != null){
//                     inventoryitem.TotalAllocated += y.Amount - x.Amount;
//                 }
//                 x.Amount = y.Amount;
//                 break;
//             }
//         }
//     }

//     //STAP 2 het toevoegen van nieuwe items aan de huidige order
//     foreach (var y in orderitems)
//     {
//         var existingItem = currentitems.FirstOrDefault(i => i.ItemId == y.ItemId);
//         {

//         }
//     }

// }
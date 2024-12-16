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

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // Haal een specifieke order op inclusief de items
        public async Task<OrderWithItemsDTO> GetOrderWithItems(int orderId)
        {
            // Gebruik LINQ om de order op te halen en deze direct te mappen naar een DTO
            return await _context.Orders
                .Where(o => o.Id == orderId)
                .Select(o => MapOrderToDTO(o)) // Map de database-entiteit naar een DTO
                .FirstOrDefaultAsync();
        }

        // Haal alle orders op inclusief hun items
        public async Task<List<OrderWithItemsDTO>> GetAllOrdersWithItems()
        {
            // Map alle orders in de database naar DTO's
            return await _context.Orders
                .Select(o => MapOrderToDTO(o)) // Gebruik de MapOrderToDTO-methode
                .ToListAsync();
        }

        // Haal alle orders op voor een specifieke klant
        public async Task<List<Order>> GetOrdersForClient(int clientId)
        {
            // Filter orders op basis van klant als ontvanger of betaler
            return await _context.Orders
                .Where(o => o.BillTo == clientId || o.ShipTo == clientId)
                .ToListAsync();
        }

        // Maak een nieuwe order aan en voeg items toe
        public async Task<Order> CreateOrder(Order order, List<ItemDTO> itemDTOs)
        {
            try
            {
                // Voeg de nieuwe order toe en sla op om het ID te genereren
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Controleer of alle items geldig zijn en voldoende voorraad hebben
                foreach (var itemDto in itemDTOs)
                {
                    // Controleer of het item bestaat
                    var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);
                    if (item == null)
                    {
                        throw new Exception($"Item met Uid {itemDto.ItemId} bestaat niet.");
                    }

                    // Controleer of het item in de voorraad aanwezig is
                    var itemInventory = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == itemDto.ItemId);
                    if (itemInventory == null)
                    {
                        throw new Exception($"Geen inventaris gevonden voor item met Uid {itemDto.ItemId}.");
                    }

                    // Controleer of er voldoende voorraad beschikbaar is
                    if (itemInventory.TotalAvailable < itemDto.Amount)
                    {
                        throw new Exception($"Niet genoeg voorraad voor item met Uid {itemDto.ItemId}. Beschikbaar: {itemInventory.TotalAvailable}, gevraagd: {itemDto.Amount}");
                    }
                }

                // Alle items zijn geldig, voeg nu de order-items toe
                foreach (var itemDto in itemDTOs)
                {
                    var itemInventory = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == itemDto.ItemId);

                    // Verminder de beschikbare voorraad
                    itemInventory.TotalAvailable -= itemDto.Amount;

                    // Maak een nieuw OrderItem aan
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id, // Order-ID is nu beschikbaar
                        ItemUid = itemDto.ItemId,
                        Amount = itemDto.Amount
                    };

                    // Voeg het item toe aan de context
                    _context.OrderItems.Add(orderItem);
                }

                // Sla de order-items en voorraadwijzigingen op in de database
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het aanmaken van een order: {ex.Message}");
                throw;
            }
        }



        // Update een bestaande order en de gekoppelde items
        public async Task<string> UpdateOrder(int orderId, OrderWithItemsDTO updatedOrderDto)
        {
            // Haal de bestaande order op
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (existingOrder == null)
            {
                return $"Order met ID {orderId} is niet gevonden.";
            }

            // Update de velden van de bestaande order
            UpdateOrderFields(existingOrder, updatedOrderDto);

            // Update de items in de order
            var updateItemsResult = await UpdateItemsInOrder(orderId, updatedOrderDto.Items);
            if (!updateItemsResult.StartsWith("Order met ID"))
            {
                return updateItemsResult;
            }

            // Sla de wijzigingen op in de database
            await _context.SaveChangesAsync();
            return $"Order met ID {orderId} is succesvol bijgewerkt.";
        }

        // Update alleen de items van een bestaande order
        public async Task<string> UpdateItemsInOrder(int orderId, List<ItemDTO> orderItems)
        {
            // Controleer of elk opgegeven item bestaat
            foreach (var item in orderItems)
            {
                if (!await ItemExist(item.ItemId)) // Controleer of het item bestaat in de database
                {
                    return $"Item met ID {item.ItemId} is niet gevonden.";
                }
            }

            // Haal de bestaande order inclusief de gekoppelde items op
            var updateOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (updateOrder == null)
            {
                return $"Order met ID {orderId} is niet gevonden.";
            }

            // Update bestaande items of verwijder ze indien ze niet meer in de order voorkomen
            foreach (var currentItem in updateOrder.OrderItems.ToList()) // Doorloop alle huidige items in de order
            {
                var matchingItem = orderItems.FirstOrDefault(i => i.ItemId == currentItem.ItemUid); // Zoek het overeenkomende item in de nieuwe lijst
                if (matchingItem != null)
                {
                    // Update voorraad voor het item als het nog bestaat
                    var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == currentItem.ItemUid);
                    if (inventoryItem != null)
                    {
                        inventoryItem.TotalAllocated += matchingItem.Amount - currentItem.Amount; // Pas de allocatie aan op basis van de hoeveelheidwijziging
                    }
                    currentItem.Amount = matchingItem.Amount; // Update de hoeveelheid van het item in de order
                }
                else
                {
                    // Verwijder het item als het niet meer nodig is
                    var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == currentItem.ItemUid);
                    if (inventoryItem != null)
                    {
                        inventoryItem.TotalAllocated -= currentItem.Amount; // Verminder de allocatie in de voorraad
                    }
                    _context.OrderItems.Remove(currentItem); // Verwijder het item uit de order
                }
            }

            // Voeg nieuwe items toe aan de order
            foreach (var newItem in orderItems)
            {
                var existingItem = updateOrder.OrderItems.FirstOrDefault(i => i.ItemUid == newItem.ItemId); // Controleer of het item al in de order zit
                if (existingItem == null) // Als het item nog niet bestaat
                {
                    var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == newItem.ItemId); // Haal het item op uit de voorraad
                    if (inventoryItem != null)
                    {
                        // Controleer of er voldoende voorraad beschikbaar is
                        if (inventoryItem.TotalAvailable < newItem.Amount)
                        {
                            return $"Niet genoeg voorraad voor item met ID {newItem.ItemId}. Beschikbaar: {inventoryItem.TotalAvailable}, gevraagd: {newItem.Amount}";
                        }

                        // Maak een nieuw orderitem aan en voeg het toe
                        var orderItem = new OrderItem
                        {
                            ItemUid = newItem.ItemId,
                            Amount = newItem.Amount,
                            OrderId = orderId
                        };

                        _context.OrderItems.Add(orderItem); // Voeg het nieuwe item toe aan de order
                        inventoryItem.TotalAllocated += newItem.Amount; // Verhoog de allocatie in de voorraad
                    }
                }
            }

            await _context.SaveChangesAsync(); // Sla alle wijzigingen op in de database
            return $"Order met ID {orderId} is succesvol bijgewerkt."; // Retourneer een succesbericht
        }


        // Controleer of een item bestaat
        public async Task<bool> ItemExist(string itemId)
        {
            return await _context.Items.AnyAsync(i => i.Uid == itemId);
        }

        // Verwijder een order en herstel de voorraad van de items
        public async Task<bool> DeleteOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Item)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return false;

            foreach (var orderItem in order.OrderItems)
            {
                var item = orderItem.Item;
                if (item != null)
                {
                    item.UnitOrderQuantity += orderItem.Amount;
                }
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        // Map een database-order naar een DTO inclusief de items
        private OrderWithItemsDTO MapOrderToDTO(Order order)
        {
            // Maak een DTO van een database-entiteit
            return new OrderWithItemsDTO
            {
                Id = order.Id,
                SourceId = order.SourceId,
                OrderDate = order.OrderDate,
                RequestDate = order.RequestDate,
                Reference = order.Reference,
                ReferenceExtra = order.ExtrReference,
                OrderStatus = order.OrderStatus,
                Notes = order.Notes,
                ShippingNotes = order.ShippingNotes,
                PickingNotes = order.PickingNotes,
                WarehouseId = order.WarehouseId,
                ShipTo = order.ShipTo,
                BillTo = order.BillTo,
                ShipmentId = order.ShipmentId,
                TotalAmount = order.TotalAmount,
                TotalDiscount = order.TotalDiscount,
                TotalTax = order.TotalTax,
                TotalSurcharge = order.TotalSurcharge,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = order.OrderItems.Select(oi => new ItemDTO
                {
                    // Map elk OrderItem naar een ItemDTO
                    ItemId = oi.Item.Uid,
                    Amount = oi.Amount
                }).ToList() // Zet de verzameling om naar een lijst
            };
        }

        // Update de velden van een bestaande order
        private void UpdateOrderFields(Order existingOrder, OrderWithItemsDTO updatedOrderDto)
        {
            existingOrder.SourceId = updatedOrderDto.SourceId;
            existingOrder.OrderDate = updatedOrderDto.OrderDate;
            existingOrder.RequestDate = updatedOrderDto.RequestDate;
            existingOrder.Reference = updatedOrderDto.Reference;
            existingOrder.ExtrReference = updatedOrderDto.ReferenceExtra;
            existingOrder.OrderStatus = updatedOrderDto.OrderStatus;
            existingOrder.Notes = updatedOrderDto.Notes;
            existingOrder.ShippingNotes = updatedOrderDto.ShippingNotes;
            existingOrder.PickingNotes = updatedOrderDto.PickingNotes;
            existingOrder.WarehouseId = updatedOrderDto.WarehouseId;
            existingOrder.ShipTo = updatedOrderDto.ShipTo;
            existingOrder.BillTo = updatedOrderDto.BillTo;
            existingOrder.ShipmentId = updatedOrderDto.ShipmentId;
            existingOrder.TotalAmount = updatedOrderDto.TotalAmount;
            existingOrder.TotalDiscount = updatedOrderDto.TotalDiscount;
            existingOrder.TotalTax = updatedOrderDto.TotalTax;
            existingOrder.TotalSurcharge = updatedOrderDto.TotalSurcharge;
        }
    }
}
/*
Waarom het gebruik van DTO's bij dit model?

1. **Flexibiliteit**:
   - DTO's bieden flexibiliteit om de structuur van de API-responses aan te passen zonder de onderliggende database- of modelstructuur te wijzigen. Dit maakt het eenvoudiger om API's te evolueren zonder grote wijzigingen in de database.

2. **Efficiëntie**:
   - Met DTO's kun je alleen de noodzakelijke gegevens ophalen die nodig zijn voor een bepaalde operatie. Dit vermindert de hoeveelheid data die wordt overgedragen en zorgt voor betere prestaties.

3. **Specifieke Data Weergave**:
   - DTO's bieden de mogelijkheid om gerichte en samengevoegde data te retourneren, zoals in het geval van `OrderWithItemsDTO`, waar een overzicht van de order wordt gegeven inclusief een lijst met items.
*/

//     Stel dat je een lijst van items in een order moet retourneren. Zonder een DTO zou je mogelijk de volledige `Item`-entiteit moeten retourneren, wat overbodige informatie bevat zoals:
//      - `SupplierId`
//      - `UnitOrderQuantity`
//      - `ShortDescription`
//      - Navigatie-eigenschappen zoals `OrderItems`
//      - Dit maakt de API-responses onnodig groot en complex.
//    - Met `ItemDTO` beperk je de data tot bijvoorbeeld:
//      ```json
//      {
//        "itemId": "ITEM001",
//        "amount": 5
//      }

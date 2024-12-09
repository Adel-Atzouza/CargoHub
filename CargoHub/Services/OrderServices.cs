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
                // Voeg de nieuwe order toe aan de databasecontext
                _context.Orders.Add(order);

                // Sla de order op zodat een ID wordt gegenereerd
                await _context.SaveChangesAsync();

                // Voeg elk item toe aan de order
                foreach (var itemDto in itemDTOs)
                {
                    await AddOrderItem(order.Id, itemDto);
                }

                // Sla alle wijzigingen op in de database
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
                if (!await ItemExist(item.ItemId))
                {
                    return $"Item met ID {item.ItemId} is niet gevonden.";
                }
            }

            // Haal de bestaande order inclusief items op
            var updateOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (updateOrder == null)
            {
                return $"Order met ID {orderId} is niet gevonden.";
            }

            // Update bestaande items of verwijder ze indien niet meer nodig
            foreach (var currentItem in updateOrder.OrderItems.ToList())
            {
                var matchingItem = orderItems.FirstOrDefault(i => i.ItemId == currentItem.ItemUid);
                if (matchingItem != null)
                {
                    var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == currentItem.ItemUid);
                    if (inventoryItem != null)
                    {
                        inventoryItem.TotalAllocated += matchingItem.Amount - currentItem.Amount;
                    }
                    currentItem.Amount = matchingItem.Amount;
                }
                else
                {
                    var inventoryItem = await _context.Inventory.FirstOrDefaultAsync(i => i.ItemId == currentItem.ItemUid);
                    if (inventoryItem != null)
                    {
                        inventoryItem.TotalAllocated -= currentItem.Amount;
                    }
                    _context.OrderItems.Remove(currentItem);
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

                        _context.OrderItems.Add(orderItem);
                        inventoryItem.TotalAllocated += newItem.Amount;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return $"Order met ID {orderId} is succesvol bijgewerkt.";
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

        // Voeg een item toe aan een order
        private async Task AddOrderItem(int orderId, ItemDTO itemDto)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Uid == itemDto.ItemId);
            if (item == null)
            {
                throw new Exception($"Item met Uid {itemDto.ItemId} bestaat niet.");
            }

            if (item.UnitOrderQuantity < itemDto.Amount)
            {
                throw new Exception($"Niet genoeg voorraad voor item met Uid: {itemDto.ItemId}. Beschikbaar: {item.UnitOrderQuantity}, gevraagd: {itemDto.Amount}");
            }

            item.UnitOrderQuantity -= itemDto.Amount;

            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ItemUid = item.Uid,
                Amount = itemDto.Amount
            };

            _context.OrderItems.Add(orderItem);
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

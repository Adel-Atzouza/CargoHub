using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CargoHub.Models;
using CargoHub.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CargoHub.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private AppDbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AppDbContext(options);
            _orderService = new OrderService(_dbContext); 
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task CreateOrder_ShouldAddOrderToDatabase()
        {
            // Arrange
            var newOrder = new Order
            {
                Reference = "Test Order",
                TotalAmount = 100.0m 
            };

            // Act
            var result = await _orderService.CreateOrder(newOrder, new List<ItemDTO>());

            // Assert
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.AreEqual("Test Order", result.Reference); // Check that the order reference matches

            var ordersInDb = await _dbContext.Orders.ToListAsync();
            Assert.AreEqual(1, ordersInDb.Count); // Ensure there is exactly one order in the database
        }

        [TestMethod]
        public async Task GetOrderWithItems_ShouldReturnCorrectOrder()
        {
            // Arrange
            _dbContext.Items.AddRange(new List<Item>
            {
                new Item { Uid = "ITEM001", Code = "Code1", Description = "Item 1" },
                new Item { Uid = "ITEM002", Code = "Code2", Description = "Item 2" }
            });
            await _dbContext.SaveChangesAsync();

            var newOrder = new Order
            {
                Reference = "Test Order",
                TotalAmount = 100.0m,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ItemUid = "ITEM001", Amount = 2 },
                    new OrderItem { ItemUid = "ITEM002", Amount = 1 }
                }
            };
            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            var orderId = newOrder.Id;

            // Act
            var result = await _orderService.GetOrderWithItems(orderId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Order", result.Reference);
            Assert.AreEqual(2, result.Items.Count);
        }

        [TestMethod]
        public async Task GetOrderWithItems_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Act
            var result = await _orderService.GetOrderWithItems(999);

            // Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task UpdateOrder_ShouldUpdateOrderFieldsAndItems()
        {
            // Arrange
            var order = new Order
            {
                Reference = "Test Order",
                TotalAmount = 200.0m,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ItemUid = "ITEM001", Amount = 2 },
                    new OrderItem { ItemUid = "ITEM002", Amount = 1 }
                }
            };

            _dbContext.Orders.Add(order);

            _dbContext.Items.AddRange(new List<Item>
            {
                new Item { Uid = "ITEM001", Description = "Test Item 1" },
                new Item { Uid = "ITEM002", Description = "Test Item 2" },
                new Item { Uid = "ITEM003", Description = "Test Item 3" }
            });

            _dbContext.Inventory.AddRange(new List<Inventory>
            {
                new Inventory { ItemId = "ITEM001", TotalAllocated = 2, TotalAvailable = 8, Description = "Inventory for ITEM001", ItemReference = "REF001" },
                new Inventory { ItemId = "ITEM002", TotalAllocated = 1, TotalAvailable = 4, Description = "Inventory for ITEM002", ItemReference = "REF002" },
                new Inventory { ItemId = "ITEM003", TotalAllocated = 0, TotalAvailable = 10, Description = "Inventory for ITEM003", ItemReference = "REF003" }
            });

            await _dbContext.SaveChangesAsync();

            var updatedOrderDto = new OrderWithItemsDTO
            {
                SourceId = 2,
                OrderDate = DateTime.UtcNow,
                RequestDate = DateTime.UtcNow.AddDays(1),
                Reference = "Updated Test Order",
                ReferenceExtra = "Extra Reference",
                OrderStatus = "Updated",
                Notes = "Updated Notes",
                ShippingNotes = "Updated Shipping Notes",
                PickingNotes = "Updated Picking Notes",
                WarehouseId = null,
                ShipTo = null,
                BillTo = null,
                ShipmentId = null,
                TotalAmount = 250.0m,
                TotalDiscount = 20.0m,
                TotalTax = 10.0m,
                TotalSurcharge = 5.0m,
                Items = new List<ItemDTO>
                {
                    new ItemDTO { ItemId = "ITEM001", Amount = 5 },
                    new ItemDTO { ItemId = "ITEM002", Amount = 3 },
                    new ItemDTO { ItemId = "ITEM003", Amount = 4 }
                }
            };

            // Act
            var result = await _orderService.UpdateOrder(order.Id, updatedOrderDto);

            // Assert
            Assert.AreEqual($"Order met ID {order.Id} is succesvol bijgewerkt.", result);

            // Controleer dat de ordervelden zijn bijgewerkt
            var updatedOrder = await _dbContext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == order.Id);
            Assert.IsNotNull(updatedOrder);
            Assert.AreEqual(2, updatedOrder.SourceId);
            Assert.AreEqual("Updated Test Order", updatedOrder.Reference);
            Assert.AreEqual("Extra Reference", updatedOrder.ExtrReference);
            Assert.AreEqual("Updated", updatedOrder.OrderStatus);
            Assert.AreEqual("Updated Notes", updatedOrder.Notes);
            Assert.AreEqual("Updated Shipping Notes", updatedOrder.ShippingNotes);
            Assert.AreEqual("Updated Picking Notes", updatedOrder.PickingNotes);
            Assert.AreEqual(250.0m, updatedOrder.TotalAmount);
            Assert.AreEqual(20.0m, updatedOrder.TotalDiscount);
            Assert.AreEqual(10.0m, updatedOrder.TotalTax);
            Assert.AreEqual(5.0m, updatedOrder.TotalSurcharge);

            // Controleer dat de items correct zijn bijgewerkt
            Assert.AreEqual(3, updatedOrder.OrderItems.Count);
            Assert.AreEqual(5, updatedOrder.OrderItems.First(i => i.ItemUid == "ITEM001").Amount);
            Assert.AreEqual(3, updatedOrder.OrderItems.First(i => i.ItemUid == "ITEM002").Amount);
            Assert.AreEqual(4, updatedOrder.OrderItems.First(i => i.ItemUid == "ITEM003").Amount);

            // Controleer dat de voorraad correct is aangepast
            var inventoryItem1 = await _dbContext.Inventory.FirstOrDefaultAsync(i => i.ItemId == "ITEM001");
            var inventoryItem2 = await _dbContext.Inventory.FirstOrDefaultAsync(i => i.ItemId == "ITEM002");
            var inventoryItem3 = await _dbContext.Inventory.FirstOrDefaultAsync(i => i.ItemId == "ITEM003");

            Assert.AreEqual(5, inventoryItem1.TotalAllocated);
            Assert.AreEqual(3, inventoryItem2.TotalAllocated);
            Assert.AreEqual(4, inventoryItem3.TotalAllocated);
        }




        [TestMethod]
        public async Task DeleteOrder_ShouldRemoveOrder_WhenOrderExists()
        {
            // Arrange
            var newOrder = new Order
            {
                Reference = "Order to Delete",
                TotalAmount = 200.0m 
            };

            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            var orderId = newOrder.Id; 

            // Act
            var result = await _orderService.DeleteOrder(orderId);

            // Assert
            Assert.IsTrue(result);

            var orderInDb = await _dbContext.Orders.FindAsync(orderId);
            Assert.IsNull(orderInDb);
        }

        [TestMethod]
        public async Task DeleteOrder_ShouldReturnFalse_WhenOrderDoesNotExist()
        {
            // Act
            var result = await _orderService.DeleteOrder(999);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

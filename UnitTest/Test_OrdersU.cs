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
            // In-memory database configureren
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AppDbContext(options);
            _orderService = new OrderService(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Database resetten na elke test
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
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Order", result.Reference);

            var ordersInDb = await _dbContext.Orders.ToListAsync();
            Assert.AreEqual(1, ordersInDb.Count);
        }

        [TestMethod]
        public async Task GetOrderWithItems_ShouldReturnCorrectOrder()
        {
            // Arrange: Voeg items toe aan de database
            _dbContext.Items.AddRange(new List<Item>
            {
                new Item { Uid = "ITEM001", Code = "Code1", Description = "Item 1" },
                new Item { Uid = "ITEM002", Code = "Code2", Description = "Item 2" }
            });
            await _dbContext.SaveChangesAsync();

            // Voeg een nieuwe order met items toe
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
            var result = await _orderService.GetOrderWithItems(999); // Gebruik een niet-bestaand ID

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteOrder_ShouldRemoveOrder_WhenOrderExists()
        {
            // Arrange: Voeg een order toe aan de database
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
            var result = await _orderService.DeleteOrder(999); // Gebruik een niet-bestaand ID

            // Assert
            Assert.IsFalse(result);
        }
    }
}

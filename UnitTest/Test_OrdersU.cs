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
        private OrderService _orderService; // Service under test
        private AppDbContext _dbContext; // In-memory database context for testing

        [TestInitialize] // This method runs before each test
        public void Setup()
        {
            // Configure an in-memory database for testing purposes
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Create a new in-memory database for each test
                .Options;

            _dbContext = new AppDbContext(options); // Initialize the database context
            _orderService = new OrderService(_dbContext); // Initialize the service with the context
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Reset the in-memory database after each test to prevent data leakage
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose(); // Dispose of the context to release resources
        }

        [TestMethod]
        public async Task CreateOrder_ShouldAddOrderToDatabase()
        {
            // Arrange: Set up a new order to be created
            var newOrder = new Order
            {
                Reference = "Test Order",
                TotalAmount = 100.0m // Set total amount for the order
            };

            // Act: Call the service to create the order with no items
            var result = await _orderService.CreateOrder(newOrder, new List<ItemDTO>());

            // Assert: Verify that the order is created successfully
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.AreEqual("Test Order", result.Reference); // Check that the order reference matches

            // Verify that the order has been added to the database
            var ordersInDb = await _dbContext.Orders.ToListAsync();
            Assert.AreEqual(1, ordersInDb.Count); // Ensure there is exactly one order in the database
        }

        [TestMethod]
        public async Task GetOrderWithItems_ShouldReturnCorrectOrder()
        {
            // Arrange: Add items to the database
            _dbContext.Items.AddRange(new List<Item>
            {
                new Item { Uid = "ITEM001", Code = "Code1", Description = "Item 1" },
                new Item { Uid = "ITEM002", Code = "Code2", Description = "Item 2" }
            });
            await _dbContext.SaveChangesAsync(); // Save items to the database

            // Add a new order with items to the database
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
            await _dbContext.SaveChangesAsync(); // Save the order to the database

            var orderId = newOrder.Id; // Get the ID of the newly created order

            // Act: Retrieve the order with its items from the service
            var result = await _orderService.GetOrderWithItems(orderId);

            // Assert: Verify that the returned order contains the correct data
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.AreEqual("Test Order", result.Reference); // Verify the reference matches
            Assert.AreEqual(2, result.Items.Count); // Verify that the order contains 2 items
        }

        [TestMethod]
        public async Task GetOrderWithItems_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Act: Attempt to retrieve an order with an ID that doesn't exist
            var result = await _orderService.GetOrderWithItems(999); // Use a non-existent ID

            // Assert: Verify that the result is null, as the order doesn't exist
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteOrder_ShouldRemoveOrder_WhenOrderExists()
        {
            // Arrange: Add an order to the database that will be deleted
            var newOrder = new Order
            {
                Reference = "Order to Delete",
                TotalAmount = 200.0m // Set the total amount for the order
            };

            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync(); // Save the order to the database

            var orderId = newOrder.Id; // Get the ID of the order to be deleted

            // Act: Call the service to delete the order
            var result = await _orderService.DeleteOrder(orderId);

            // Assert: Verify that the order was deleted
            Assert.IsTrue(result); // The deletion should be successful

            // Verify that the order has been removed from the database
            var orderInDb = await _dbContext.Orders.FindAsync(orderId);
            Assert.IsNull(orderInDb); // The order should no longer be in the database
        }

        [TestMethod]
        public async Task DeleteOrder_ShouldReturnFalse_WhenOrderDoesNotExist()
        {
            // Act: Attempt to delete an order with a non-existent ID
            var result = await _orderService.DeleteOrder(999); // Use a non-existent ID

            // Assert: Verify that the deletion fails and returns false
            Assert.IsFalse(result);
        }
    }
}

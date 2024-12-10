using Microsoft.VisualStudio.TestTools.UnitTesting;
using CargoHub.Services;
using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Tests
{
    [TestClass]
    public class ShipmentServiceTests
    {
        private AppDbContext _dbContext;
        private ShipmentService _shipmentService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AppDbContext(options);
            _shipmentService = new ShipmentService(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task GetAllShipmentsWithItems_ShouldReturnShipments()
        {
            // Arrange: Voeg eerst de items toe aan de database
            _dbContext.Items.Add(new Item { Uid = "ITEM001", Description = "Test Item 1", UnitOrderQuantity = 100 });
            _dbContext.Items.Add(new Item { Uid = "ITEM002", Description = "Test Item 2", UnitOrderQuantity = 100 });
            await _dbContext.SaveChangesAsync();

            // Voeg een zending met orders en orderitems toe
            _dbContext.Shipments.Add(new Shipment
            {
                ShipmentDate = DateTime.UtcNow,
                ShipmentType = "Standard",
                ShipmentStatus = "Pending",
                Notes = "Test shipment",
                orders = new List<Order>
                {
                    new Order
                    {
                        Reference = "ORD001",
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem { ItemUid = "ITEM001", Amount = 5 },
                            new OrderItem { ItemUid = "ITEM002", Amount = 3 }
                        }
                    }
                }
            });
            await _dbContext.SaveChangesAsync();

            // Act:
            var result = await _shipmentService.GetAllShipmentsWithItems();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count); // Controleer dat er één zending is
            Assert.AreEqual("Standard", result[0].ShipmentType); // Controleer het type
            Assert.AreEqual("ORD001", result[0].Orders.First().Reference); // Controleer de referentie
            Assert.AreEqual(2, result[0].Orders.First().Items.Count); // Controleer dat er 2 items zijn
            Assert.AreEqual("ITEM001", result[0].Orders.First().Items.First().ItemId); // Controleer het eerste item
            Assert.AreEqual(5, result[0].Orders.First().Items.First().Amount); // Controleer de hoeveelheid van het eerste item
        }


        [TestMethod]
        public async Task GetShipmentByIdWithOrderDetails_ShouldReturnCorrectShipment()
        {
            // Arrange
            var shipment = new Shipment
            {
                ShipmentDate = DateTime.UtcNow,
                ShipmentType = "Express",
                ShipmentStatus = "Delivered",
                Notes = "Urgent",
                CarrierCode = "C456",
                CarrierDescription = "Premium Carrier",
                orders = new List<Order>
                {
                    new Order
                    {
                        Reference = "ORD002",
                        OrderDate = DateTime.UtcNow,
                        RequestDate = DateTime.UtcNow.AddDays(1),
                        OrderStatus = "Delivered"
                    }
                }
            };

            _dbContext.Shipments.Add(shipment);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _shipmentService.GetShipmentByIdWithOrderDetails(shipment.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(shipment.Id, result.Id);
            Assert.AreEqual("Express", result.ShipmentType);
            Assert.AreEqual("ORD002", result.Orders.First().Reference);
        }

        [TestMethod]
        public async Task CreateShipment_ShouldAddShipmentToDatabase()
        {
            // Arrange
            var shipment = new Shipment
            {
                ShipmentDate = DateTime.UtcNow,
                ShipmentType = "Economy",
                ShipmentStatus = "Pending",
                Notes = "No rush",
                CarrierCode = "C789",
                TotalPackageCount = 20,
                TotalPackageWeight = 100.0m
            };

            // Act
            var result = await _shipmentService.CreateShipment(shipment);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Economy", result.ShipmentType);
            Assert.AreEqual(1, _dbContext.Shipments.Count());
        }

        [TestMethod]
        public async Task DeleteShipment_ShouldRemoveShipment()
        {
            // Arrange
            var shipment = new Shipment
            {
                ShipmentDate = DateTime.UtcNow,
                ShipmentType = "Freight",
                ShipmentStatus = "Completed"
            };

            _dbContext.Shipments.Add(shipment);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _shipmentService.DeleteShipment(shipment.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, _dbContext.Shipments.Count());
        }

        [TestMethod]
        public async Task DeleteShipment_ShouldRemoveShipmentAndUnlinkOrders()
        {
            // Arrange
            var shipment = new Shipment
            {
                ShipmentDate = DateTime.UtcNow,
                ShipmentType = "Freight",
                ShipmentStatus = "Completed",
                orders = new List<Order>
                {
                    new Order { Reference = "ORD001", OrderStatus = "Packed", ShipmentId = 1 },
                    new Order { Reference = "ORD002", OrderStatus = "Packed", ShipmentId = 1 }
                }
            };

            _dbContext.Shipments.Add(shipment);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _shipmentService.DeleteShipment(shipment.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, _dbContext.Shipments.Count());

            // Controleer of de orders zijn losgekoppeld van de zending
            var orders = await _dbContext.Orders.ToListAsync();
            foreach (var order in orders)
            {
                Assert.IsNull(order.ShipmentId);
                Assert.AreEqual("Scheduled", order.OrderStatus);
            }
        }


        [TestMethod]
        public async Task AssignOrdersToShipment_ShouldLinkOrdersToShipment()
        {
            // Arrange
            var shipment = new Shipment
            {
                ShipmentDate = DateTime.UtcNow,
                ShipmentType = "Ground",
                ShipmentStatus = "Scheduled"
            };

            var orders = new List<Order>
            {
                new Order { Reference = "ORD003", OrderDate = DateTime.UtcNow },
                new Order { Reference = "ORD004", OrderDate = DateTime.UtcNow }
            };

            _dbContext.Shipments.Add(shipment);
            _dbContext.Orders.AddRange(orders);
            await _dbContext.SaveChangesAsync();

            var orderIds = orders.Select(o => o.Id).ToList();

            // Act
            var result = await _shipmentService.AssignOrdersToShipment(shipment.Id, orderIds);

            // Assert
            Assert.IsTrue(result);
            var updatedOrders = await _dbContext.Orders.Where(o => o.ShipmentId == shipment.Id).ToListAsync();
            Assert.AreEqual(2, updatedOrders.Count);
        }

        [TestMethod]
        public async Task UpdateShipmentFields_ShouldThrowError_WhenChangingStatusToPendingFromTransitOrDelivered()
        {
            // Arrange
            var shipment = new Shipment
            {
                Id = 1,
                ShipmentStatus = "transit",
                orders = new List<Order>
                {
                    new Order { Id = 1, OrderStatus = "processing" }
                }
            };
            _dbContext.Shipments.Add(shipment);
            await _dbContext.SaveChangesAsync();
        
            var updatedShipmentDto = new ShipmentDTO
            {
                ShipmentStatus = "pending"
            };
        
            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _shipmentService.UpdateShipmentFields(shipment.Id, updatedShipmentDto);
            });
        
            Assert.AreEqual("Cannot change shipment status to pending from transit or delivered.", exception.Message);
        }
        
        [TestMethod]
        public async Task UpdateShipmentFields_ShouldUpdateShipment_WhenValidDataProvided()
        {
            // Arrange
            var shipment = new Shipment
            {
                Id = 1,
                ShipmentStatus = "pending",
                orders = new List<Order>
                {
                    new Order { Id = 1, OrderStatus = "processing" }
                }
            };
            _dbContext.Shipments.Add(shipment);
            await _dbContext.SaveChangesAsync();
        
            var updatedShipmentDto = new ShipmentDTO
            {
                ShipmentStatus = "transit",
                ShipmentType = "Express",
                Notes = "Updated notes",
                CarrierCode = "C123",
                CarrierDescription = "Updated Carrier",
                ServiceCode = "S123",
                PaymentType = "Prepaid",
                TransferMode = "Air",
                TotalPackageCount = 10,
                TotalPackageWeight = 100
            };
        
            // Act
            var result = await _shipmentService.UpdateShipmentFields(shipment.Id, updatedShipmentDto);
        
            // Assert
            Assert.AreEqual($"Shipment met ID {shipment.Id} is succesvol bijgewerkt.", result);
        
            var updatedShipment = await _dbContext.Shipments.FindAsync(shipment.Id);
            Assert.AreEqual("transit", updatedShipment.ShipmentStatus);
            Assert.AreEqual("Express", updatedShipment.ShipmentType);
            Assert.AreEqual("Updated notes", updatedShipment.Notes);
            Assert.AreEqual("C123", updatedShipment.CarrierCode);
            Assert.AreEqual("Updated Carrier", updatedShipment.CarrierDescription);
            Assert.AreEqual("S123", updatedShipment.ServiceCode);
            Assert.AreEqual("Prepaid", updatedShipment.PaymentType);
            Assert.AreEqual("Air", updatedShipment.TransferMode);
            Assert.AreEqual(10, updatedShipment.TotalPackageCount);
            Assert.AreEqual(100, updatedShipment.TotalPackageWeight);
            Assert.AreEqual("shipped", updatedShipment.orders.First().OrderStatus);
        }
    }
}

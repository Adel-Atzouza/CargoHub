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
public class MonthlyReportTests
{
    private ReportService _reportService; // Service under test
    private AppDbContext _dbContext; // In-memory database context for testing

    [TestInitialize] // This method runs before each test
    public void Setup()
    {
        // Configure an in-memory database for testing purposes
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Create a new in-memory database for each test
            .Options;

        _dbContext = new AppDbContext(options); // Initialize the database context
        _reportService = new ReportService(_dbContext); // Initialize the service with the context
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Reset the in-memory database after each test to prevent data leakage
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose(); // Dispose of the context to release resources
    }

    [TestMethod]
    public async Task GenerateMonthlyReport_ShouldCalculateCorrectMetrics()
    {
        // Arrange
        _dbContext.Orders.AddRange(new List<Order>
        {
            new Order 
            { 
                Reference = "ORD001", 
                OrderStatus = "Packed", 
                TotalAmount = 150, 
                RequestDate = new DateTime(2024, 12, 5), 
                CreatedAt = new DateTime(2024, 12, 1) // Set CreatedAt in October
            },
            new Order 
            { 
                Reference = "ORD002", 
                OrderStatus = "Packed", 
                TotalAmount = 250, 
                RequestDate = new DateTime(2024, 12, 7), 
                CreatedAt = new DateTime(2024, 12, 3) // Set CreatedAt in October
            }
        });


        _dbContext.Shipments.AddRange(new List<Shipment>
        {
            new Shipment 
            { 
                CreatedAt = new DateTime(2024, 10, 1), 
                ShipmentDate = new DateTime(2024, 10, 4), 
                ShipmentStatus = "Transit", 
                TotalPackageWeight = 100 
            },
            new Shipment 
            { 
                CreatedAt = new DateTime(2024, 10, 2), 
                ShipmentDate = new DateTime(2024, 10, 6), 
                ShipmentStatus = "Delivered", 
                TotalPackageWeight = 200 
            }
        });

        await _dbContext.SaveChangesAsync();

        var allOrders = await _dbContext.Orders.ToListAsync();
        var allShipments = await _dbContext.Shipments.ToListAsync();

        Console.WriteLine($"Orders in DB: {allOrders.Count}");
        allOrders.ForEach(o => Console.WriteLine($"Order: {o.Reference}, CreatedAt: {o.CreatedAt}, RequestDate: {o.RequestDate}"));

        Console.WriteLine($"Shipments in DB: {allShipments.Count}");
        allShipments.ForEach(s => Console.WriteLine($"Shipment: {s.Id}, CreatedAt: {s.CreatedAt}, ShipmentDate: {s.ShipmentDate}"));
        

        // Act
        var report = await _reportService.GenerateMonthlyReport(2024, 10);
        Console.WriteLine($"Total Orders: {report.TotalOrders}");
        Console.WriteLine($"Total Order Amount: {report.TotalOrderAmount}");
        Console.WriteLine($"Average Order Processing Time: {report.AverageOrderProcessingTime}");
        Console.WriteLine($"Total Shipments: {report.TotalShipments}");
        Console.WriteLine($"Average Shipment Transit Time: {report.AverageShipmentTransitProcessingTime}");


        // Assert
        Assert.AreEqual(2, report.TotalOrders);
        Assert.AreEqual(400, report.TotalOrderAmount);
        Assert.AreEqual(4, report.AverageOrderProcessingTime);
        Assert.AreEqual(2, report.TotalShipments);
        Assert.AreEqual(3.5, report.AverageShipmentTransitProcessingTime);
    }

}

}

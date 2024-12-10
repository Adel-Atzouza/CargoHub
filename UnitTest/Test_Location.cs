using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargoHub.Models;
using CargoHub.Services;

namespace CargoHub.Tests
{
    [TestClass]
    public class LocationServiceTests
    {
        private LocationService _locationService; 
        private AppDbContext _dbContext;

        [TestInitialize] // Setup method that runs before each test
        public void Setup()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Create an in-memory database
                .Options;

            _dbContext = new AppDbContext(options);
            _locationService = new LocationService(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task GetAllLocations_ShouldReturnAllLocations()
        {
            // Arrange
            _dbContext.Locations.AddRange(new List<Location>
            {
                new Location { Name = "Location 1", Code = "LOC001" },
                new Location { Name = "Location 2", Code = "LOC002" }
            });
            await _dbContext.SaveChangesAsync(); // Save changes to the database

            // Act
            var result = await _locationService.GetAllLocations();

            // Assert
            Assert.AreEqual(2, result.Count); // The result should contain 2 locations
        }

        [TestMethod]
        public async Task GetLocation_ShouldReturnCorrectLocation()
        {
            // Arrange
            var location = new Location { Name = "Test Location", Code = "LOC001" };
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync(); // Save changes to the database

            // Act
            var result = await _locationService.GetLocation(location.Id);

            // Assert
            Assert.IsNotNull(result); // Ensure that the result is not null
            Assert.AreEqual("Test Location", result.Name); // Check if the name is correct
            Assert.AreEqual("LOC001", result.Code); // Check if the code is correct
        }

        [TestMethod]
        public async Task GetLocation_ShouldReturnNull_WhenLocationDoesNotExist()
        {
            // Act
            var result = await _locationService.GetLocation(999);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task AddLocation_ShouldAddLocationToDatabase()
        {
            // Arrange
            var newLocation = new Location
            {
                Name = "New Location",
                Code = "LOC001"
            };

            // Act
            var result = await _locationService.AddLocation(newLocation);

            // Assert
            Assert.AreEqual("Locatie succesvol toegevoegd.", result); // Check for success message

            var locations = await _dbContext.Locations.ToListAsync(); // Retrieve all locations from the database
            Assert.AreEqual(1, locations.Count); // Verify that the location count is 1
            Assert.AreEqual("New Location", locations[0].Name); // Verify that the added location has the correct name
        }

        [TestMethod]
        public async Task AddLocation_ShouldReturnError_WhenWarehouseIdIsInvalid()
        {
            // Arrange
            var newLocation = new Location
            {
                Name = "Invalid Location",
                Code = "LOC002",
                WarehouseId = 999
            };

            // Act
            var result = await _locationService.AddLocation(newLocation);

            // Assert
            Assert.AreEqual("Ongeldige WarehouseId opgegeven.", result);

            var locations = await _dbContext.Locations.ToListAsync(); // Retrieve all locations from the database
            Assert.AreEqual(0, locations.Count); // No locations should be added due to the error
        }

        [TestMethod]
        public async Task UpdateLocation_ShouldUpdateExistingLocation()
        {
            // Arrange
            var location = new Location { Name = "Old Name", Code = "LOC001" };
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync(); // Save changes to the database

            var updatedLocation = new Location { Name = "New Name", Code = "LOC001" };

            // Act
            var result = await _locationService.UpdateLocation(location.Id, updatedLocation);

            // Assert
            Assert.AreEqual("Locatie succesvol bijgewerkt.", result); // Check for success message

            var updated = await _dbContext.Locations.FindAsync(location.Id); // Retrieve the updated location
            Assert.AreEqual("New Name", updated.Name); // Verify that the name is updated
        }

        [TestMethod]
        public async Task UpdateLocation_ShouldReturnError_WhenLocationDoesNotExist()
        {
            // Arrange
            var updatedLocation = new Location { Name = "Non-existent Location", Code = "LOC001" };

            // Act
            var result = await _locationService.UpdateLocation(999, updatedLocation);

            // Assert
            Assert.AreEqual("Fout: Locatie niet gevonden.", result);
        }

        [TestMethod]
        public async Task RemoveLocation_ShouldDeleteLocation()
        {
            // Arrange
            var location = new Location { Name = "Location to Delete", Code = "LOC003" };
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync(); // Save changes to the database

            // Act
            var result = await _locationService.RemoveLocation(location.Id);

            // Assert
            Assert.IsTrue(result); // The deletion should be successful

            var locations = await _dbContext.Locations.ToListAsync(); // Retrieve all locations from the database
            Assert.AreEqual(0, locations.Count); // No locations should be left in the database
        }

        [TestMethod]
        public async Task RemoveLocation_ShouldReturnFalse_WhenLocationDoesNotExist()
        {
            // Act
            var result = await _locationService.RemoveLocation(999);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

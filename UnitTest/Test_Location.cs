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
        private LocationService _locationService; // The service under test
        private AppDbContext _dbContext; // In-memory database context for testing

        [TestInitialize] // Setup method that runs before each test
        public void Setup()
        {
            // Configure the in-memory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Create an in-memory database
                .Options;

            _dbContext = new AppDbContext(options); // Initialize the database context
            _locationService = new LocationService(_dbContext); // Initialize the service with the context
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Ensure the in-memory database is deleted after each test
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose(); // Dispose the database context to release resources
        }

        [TestMethod]
        public async Task GetAllLocations_ShouldReturnAllLocations()
        {
            // Arrange: Add sample locations to the in-memory database
            _dbContext.Locations.AddRange(new List<Location>
            {
                new Location { Name = "Location 1", Code = "LOC001" },
                new Location { Name = "Location 2", Code = "LOC002" }
            });
            await _dbContext.SaveChangesAsync(); // Save changes to the database

            // Act: Call the service method to get all locations
            var result = await _locationService.GetAllLocations();

            // Assert: Verify that the correct number of locations are returned
            Assert.AreEqual(2, result.Count); // The result should contain 2 locations
        }

        [TestMethod]
        public async Task GetLocation_ShouldReturnCorrectLocation()
        {
            // Arrange: Add a location to the in-memory database
            var location = new Location { Name = "Test Location", Code = "LOC001" };
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync(); // Save changes to the database

            // Act: Call the service method to retrieve the location by ID
            var result = await _locationService.GetLocation(location.Id);

            // Assert: Verify that the returned location matches the expected values
            Assert.IsNotNull(result); // Ensure that the result is not null
            Assert.AreEqual("Test Location", result.Name); // Check if the name is correct
            Assert.AreEqual("LOC001", result.Code); // Check if the code is correct
        }

        [TestMethod]
        public async Task GetLocation_ShouldReturnNull_WhenLocationDoesNotExist()
        {
            // Act: Try to retrieve a location with an invalid ID (999)
            var result = await _locationService.GetLocation(999);

            // Assert: Verify that the result is null as the location does not exist
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task AddLocation_ShouldAddLocationToDatabase()
        {
            // Arrange: Create a new location to be added
            var newLocation = new Location
            {
                Name = "New Location",
                Code = "LOC001"
            };

            // Act: Call the service method to add the location
            var result = await _locationService.AddLocation(newLocation);

            // Assert: Verify that the location was successfully added
            Assert.AreEqual("Locatie succesvol toegevoegd.", result); // Check for success message

            var locations = await _dbContext.Locations.ToListAsync(); // Retrieve all locations from the database
            Assert.AreEqual(1, locations.Count); // Verify that the location count is 1
            Assert.AreEqual("New Location", locations[0].Name); // Verify that the added location has the correct name
        }

        [TestMethod]
        public async Task AddLocation_ShouldReturnError_WhenWarehouseIdIsInvalid()
        {
            // Arrange: Create a location with an invalid WarehouseId
            var newLocation = new Location
            {
                Name = "Invalid Location",
                Code = "LOC002",
                WarehouseId = 999 // Invalid WarehouseId
            };

            // Act: Call the service method to add the location
            var result = await _locationService.AddLocation(newLocation);

            // Assert: Verify that the error message is returned due to invalid WarehouseId
            Assert.AreEqual("Ongeldige WarehouseId opgegeven.", result);

            var locations = await _dbContext.Locations.ToListAsync(); // Retrieve all locations from the database
            Assert.AreEqual(0, locations.Count); // No locations should be added due to the error
        }

        [TestMethod]
        public async Task UpdateLocation_ShouldUpdateExistingLocation()
        {
            // Arrange: Add an initial location to the database
            var location = new Location { Name = "Old Name", Code = "LOC001" };
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync(); // Save changes to the database

            var updatedLocation = new Location { Name = "New Name", Code = "LOC001" };

            // Act: Call the service method to update the location
            var result = await _locationService.UpdateLocation(location.Id, updatedLocation);

            // Assert: Verify that the location was successfully updated
            Assert.AreEqual("Locatie succesvol bijgewerkt.", result); // Check for success message

            var updated = await _dbContext.Locations.FindAsync(location.Id); // Retrieve the updated location
            Assert.AreEqual("New Name", updated.Name); // Verify that the name is updated
        }

        [TestMethod]
        public async Task UpdateLocation_ShouldReturnError_WhenLocationDoesNotExist()
        {
            // Arrange: Create an updated location with a non-existent ID
            var updatedLocation = new Location { Name = "Non-existent Location", Code = "LOC001" };

            // Act: Try to update a location that does not exist
            var result = await _locationService.UpdateLocation(999, updatedLocation);

            // Assert: Verify that an error message is returned
            Assert.AreEqual("Fout: Locatie niet gevonden.", result);
        }

        [TestMethod]
        public async Task RemoveLocation_ShouldDeleteLocation()
        {
            // Arrange: Add a location to be deleted
            var location = new Location { Name = "Location to Delete", Code = "LOC003" };
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync(); // Save changes to the database

            // Act: Call the service method to delete the location
            var result = await _locationService.RemoveLocation(location.Id);

            // Assert: Verify that the location was successfully deleted
            Assert.IsTrue(result); // The deletion should be successful

            var locations = await _dbContext.Locations.ToListAsync(); // Retrieve all locations from the database
            Assert.AreEqual(0, locations.Count); // No locations should be left in the database
        }

        [TestMethod]
        public async Task RemoveLocation_ShouldReturnFalse_WhenLocationDoesNotExist()
        {
            // Act: Try to delete a location with an invalid ID (999)
            var result = await _locationService.RemoveLocation(999);

            // Assert: Verify that the result is false since the location does not exist
            Assert.IsFalse(result);
        }
    }
}

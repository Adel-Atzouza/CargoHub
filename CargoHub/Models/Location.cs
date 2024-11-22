using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class Location
    {
        public int Id { get; set; }

        // Foreign Key for Warehouse (nullable)
        public int? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; } // Use correct capitalization for the navigation property

        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

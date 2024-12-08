using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class Location : BaseModel
    {
        public int Id { get; set; }

        // Foreign Key for Warehouse (nullable)
        public int? WarehouseId { get; set; }
        [JsonIgnore]
        public Warehouse? Warehouse { get; set; } // Navigation property with correct capitalization

        public string? Code { get; set; } // Nullable based on provided JSON
        public string? Name { get; set; } // Nullable based on provided JSON
    }
}

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
        public Warehouse? Warehouse { get; set; } // Use correct capitalization for the navigation property

        public string Code { get; set; }
        public string Name { get; set; }
    }
}

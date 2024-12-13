using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class Location : BaseModel
    {
        [JsonPropertyName("warehouse_id")]
        public int? WarehouseId { get; set; }
        [JsonIgnore]

        public Warehouse? Warehouse { get; set; } // Navigation property with correct capitalization

        [JsonPropertyName("code")]


        public string? Code { get; set; } // Nullable based on provided JSON

        [JsonPropertyName("name")]

        public string? Name { get; set; } // Nullable based on provided JSON
    }
}

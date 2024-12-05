using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class Location : BaseModel
    {
        [Required]
        public int WarehouseId { get; set; }
        [JsonIgnore]
        public Warehouse? Warehouse { get; set; }

        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class Inventory : BaseModel
    {
<<<<<<< HEAD

        [JsonPropertyName("item_id")]
        public string? ItemId { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("item_reference")]
        public string? ItemReference { get; set; }

        [JsonPropertyName("locations")]
        public List<int>? Locations { get; set; }

        [JsonPropertyName("total_on_hand")]
        public int? TotalOnHand { get; set; }

        [JsonPropertyName("total_expected")]
        public int? TotalExpected { get; set; }

        [JsonPropertyName("total_ordered")]
        public int? TotalOrdered { get; set; }

        [JsonPropertyName("total_allocated")]
        public int? TotalAllocated { get; set; }

        [JsonPropertyName("total_available")]
        public int? TotalAvailable { get; set; }
=======
        public int Id { get; set; }
        public string ItemId { get; set; }
        public string? Description { get; set; }
        public string? ItemReference { get; set; }
        public List<int>? Locations { get; set; }
        public int? TotalOnHand { get; set; }
        public int? TotalExpected { get; set; }
        public int? TotalOrdered { get; set; }
        public int? TotalAllocated { get; set; }
        public int? TotalAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
>>>>>>> origin/development
    }
}

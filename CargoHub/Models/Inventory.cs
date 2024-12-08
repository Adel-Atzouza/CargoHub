using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class Inventory : BaseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

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
    }
}

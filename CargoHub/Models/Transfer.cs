using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoHub.Models
{
    public class Transfer : BaseModel
    {
        public string? Reference { get; set; }
        [JsonPropertyName("transfer_from")]
        public int? TransferFrom { get; set; }
        [JsonPropertyName("transfer_to")]
        public int? TransferTo { get; set; }
        [JsonPropertyName("transfer_status")]
        public string? TransferStatus { get; set; }

        // Items list for JSON serialization/deserialization
        [NotMapped]
        [JsonPropertyName("items")]
        public List<Dictionary<string, object>> Items
        {
            get => ItemsJson == null ? new List<Dictionary<string, object>>() : JsonSerializer.Deserialize<List<Dictionary<string, object>>>(ItemsJson) ?? new List<Dictionary<string, object>>();
            set => ItemsJson = JsonSerializer.Serialize(value);
        }

        // Serialized JSON string for database persistence
        [JsonIgnore]
        public string ItemsJson { get; set; } = "[]";
    }
}
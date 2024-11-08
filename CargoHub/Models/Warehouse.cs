using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public record Warehouse
    {
        // Properties
        public int Id {get; set;}
        public string? Code {get; set;}
        public string? Name {get; set;}
        public string? Address {get; set;}
        public string? Zip {get; set;}
        public string? City {get; set;}
        public string? Province {get; set;}
        public string? Country {get; set;}
        
        // Contact relation
        [JsonIgnore]
        public int ContactId { get; set; }
        public Contact? Contact {get; set;}
        
        // Metadata
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt {get; set;}
        
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt {get; set;}
    }
}

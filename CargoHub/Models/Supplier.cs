using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public record Supplier
    {
        // Properties
        public Guid Id {get; set;}
        public string? Code {get; set;}
        public string? Name {get; set;}
        public string? Address {get; set;}
        [JsonPropertyName("address_extra")]
        public string? AddressExtra {get; set;}
        [JsonPropertyName("zip_code")]
        public string? ZipCode {get; set;}
        public string? City {get; set;}
        public string? Province {get; set;}
        public string? Country {get; set;}
        
        // Contact relation
        [JsonIgnore]
        public Guid ContactId { get; set; }
        public Contact? Contact {get; set;}
        
        // Metadata
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt {get; set;}
        
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt {get; set;}
    }
}

// Suppliers.json:
//  - for what is reference
//  - contact_name & phonenumber
//  - zip_code -> zip
//  - contact_name -> contact {name}
//  - phonenumber 
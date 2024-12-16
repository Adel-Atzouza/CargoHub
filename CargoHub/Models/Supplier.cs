using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public record Supplier
    {
        // Properties
        public int Id {get; set;}
        public string? Code {get; set;}
        public string? Name {get; set;}
        public string? Address {get; set;}
        [JsonPropertyName("address_extra")]
        public string? AddressExtra {get; set;}
        public string? City {get; set;}
        [JsonPropertyName("zip_code")]
        public string? ZipCode {get; set;}
        public string? Province {get; set;}
        public string? Country {get; set;}
        
        [JsonPropertyName("contact_name")]
        public string? ContactName {get; set;}
        public string? Phonenumber {get; set;}
        public string? Reference {get; set;}
        
        // Metadata
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt {get; set;}
        
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt {get; set;}
    }
}

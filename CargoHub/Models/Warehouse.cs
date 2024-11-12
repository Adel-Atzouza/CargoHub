using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public record Warehouse
    {
        // Properties
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Zip { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }

        // Contact fields (ignored in JSON)
        [JsonIgnore]
        public string? ContactName { get; set; }

        [JsonIgnore]
        public string? ContactPhone { get; set; }

        [JsonIgnore]
        public string? ContactEmail { get; set; }

        [NotMapped]
        // Contact dictionary for JSON serialization/deserialization
        [JsonPropertyName("contact")]
        public Dictionary<string, string?> Contact
        {
            get => new Dictionary<string, string?>
            {
                { "name", ContactName },
                { "phone", ContactPhone },
                { "email", ContactEmail }
            };
            set
            {
                if (value != null)
                {
                    // Safely access each key in the contact dictionary
                    ContactName = value.TryGetValue("name", out var name) ? name : null;
                    ContactPhone = value.TryGetValue("phone", out var phone) ? phone : null;
                    ContactEmail = value.TryGetValue("email", out var email) ? email : null;
                }
            }
        }

                // Metadata
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}

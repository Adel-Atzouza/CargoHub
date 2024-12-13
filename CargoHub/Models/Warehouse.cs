using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class Warehouse : BaseModel
    {
        // // Properties
        // [JsonPropertyName("id")]
        // public int Id { get; set; }
        [JsonPropertyName("code")]

        public string? Code { get; set; }
        [JsonPropertyName("name")]

        public string? Name { get; set; }
        [JsonPropertyName("address")]

        public string? Address { get; set; }
        [JsonPropertyName("zip")]

        public string? Zip { get; set; }
        [JsonPropertyName("city")]

        public string? City { get; set; }
        [JsonPropertyName("province")]

        public string? Province { get; set; }
        [JsonPropertyName("country")]

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
        // [JsonPropertyName("created_at")]
        // public DateTime CreatedAt { get; set; }

        // [JsonPropertyName("updated_at")]
        // public DateTime UpdatedAt { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class BaseModel
    {
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
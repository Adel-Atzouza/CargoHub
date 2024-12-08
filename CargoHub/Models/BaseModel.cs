using System.Text.Json.Serialization;

namespace CargoHub
{
    public class BaseModel
    {
        [JsonPropertyOrder(-1)]
        public int Id { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
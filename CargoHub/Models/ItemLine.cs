using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class ItemLine : BaseModel
    {

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]

        public string? Description { get; set; }
    }
}

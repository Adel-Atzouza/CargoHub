using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class ItemGroup : BaseModel
    {
        [JsonPropertyName("name")]
        [Required]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        [Required]
        public string? Description { get; set; }
    }
}

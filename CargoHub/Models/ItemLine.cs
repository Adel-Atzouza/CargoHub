using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class ItemLine : BaseModel
    {

        [Required]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        [Required]
        public string? Description { get; set; }
    }
}

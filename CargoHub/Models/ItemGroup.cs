using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class ItemGroup : BaseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]

        public string? Name { get; set; }

        [JsonPropertyName("description")]

        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<ItemLine> ItemLines { get; set; }
        
    }


}


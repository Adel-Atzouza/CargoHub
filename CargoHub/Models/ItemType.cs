using System.Text.Json.Serialization;
namespace CargoHub.Models
{

    public class ItemType : BaseModel
    {
        [JsonPropertyName("id")]

        public int Id { get; set; }
        [JsonPropertyName("name")]

        public string? Name { get; set; }
        [JsonPropertyName("description")]

        public string? Description { get; set; }

        // Foreign key
        // assign it for now to Item group with id 1,
        // otherwise EntityFrameWork will throw an error because it's not assigned to a valid ItemGroup
        public int ItemLineId { get; set; } = 1;

        // Navigation properties

        public ItemLine? ItemLine { get; set; }
        [JsonIgnore]

        public ICollection<Item>? Items { get; set; }
    }
}



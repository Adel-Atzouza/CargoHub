using System.Text.Json.Serialization;

namespace CargoHub
{

    public class ItemLine : BaseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        // Foreign key
        [JsonIgnore]
        public int ItemGroupId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public ItemGroup? ItemGroup { get; set; }
        
        [JsonIgnore]
        public ICollection<ItemType>? ItemTypes { get; set; }
    }
}
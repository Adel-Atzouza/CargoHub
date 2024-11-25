using System.Text.Json.Serialization;
namespace CargoHub
{

    public class ItemType : BaseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        // Foreign key
        [JsonIgnore]
        
        public int ItemLineId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public ItemLine? ItemLine { get; set; }
        [JsonIgnore]
        
        public ICollection<Item>? Items { get; set; }
    }
}



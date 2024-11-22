namespace CargoHub
{

    public class ItemLine : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Foreign key
        public int ItemGroupId { get; set; }

        // Navigation properties
        public ItemGroup ItemGroup { get; set; }
        public ICollection<ItemType> ItemTypes { get; set; }
    }
}
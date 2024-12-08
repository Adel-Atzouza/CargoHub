namespace CargoHub.Models
{
    public class ItemLine : BaseModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int ItemGroupId { get; set; }
        public ItemGroup ItemGroup { get; set; }

        // Navigation property
        public ICollection<ItemType> ItemTypes { get; set; } = new List<ItemType>();
    }
}

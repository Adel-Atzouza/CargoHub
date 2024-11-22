namespace CargoHub
{
    public class Item : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        // Foreign key
        public int ItemTypeId { get; set; }

        // Navigation property
        public ItemType ItemType { get; set; }
    }
}
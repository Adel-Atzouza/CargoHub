namespace CargoHub
{

    public class ItemType : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Foreign key
        public int ItemLineId { get; set; }

        // Navigation properties
        public ItemLine ItemLine { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}



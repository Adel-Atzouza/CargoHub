namespace CargoHub
{

    public class ItemType : BaseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}


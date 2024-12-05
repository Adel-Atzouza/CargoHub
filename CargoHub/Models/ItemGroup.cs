namespace CargoHub
{
    public class ItemGroup : BaseModel
    {
        public string? Name {get;set;}
        public string? Description {get;set;}
        
        // public ICollection<ItemLine> ItemLines { get; set; } = new List<ItemLine>();
    }
}

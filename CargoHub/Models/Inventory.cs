namespace CargoHub.Models
{
    public class Inventory : BaseModel
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
        public string ItemReference { get; set; }
        public List<int> Locations { get; set; }
        public int TotalOnHand { get; set; }
        public int TotalExpected { get; set; }
        public int TotalOrdered { get; set; }
        public int TotalAllocated { get; set; }
        public int TotalAvailable { get; set; }
    }
}

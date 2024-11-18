using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace CargoHub.Models{

public class Location{
    public int Id {get; set;}
    [JsonIgnore]
    public int? WarehouseId { get; set; }
    public Warehouse? warehouse {get;set;}
    public string Code { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
}
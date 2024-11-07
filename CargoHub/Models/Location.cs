using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Location{
    public Guid Id {get; set;}
    [JsonIgnore]
    public Guid WarehouseId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
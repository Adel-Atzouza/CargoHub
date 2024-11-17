using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models{

public class Order
{
    public int Id { get; set; }
    public int SourceId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequestDate { get; set; }
    public string Reference { get; set; }
    public string ReferenceExtra { get; set; }
    public string OrderStatus { get; set; }
    public string Notes { get; set; }
    public string ShippingNotes { get; set; }
    public string PickingNotes { get; set; }
    public int? WarehouseId { get; set; }
    public int? ShipTo { get; set; }
    public int? BillTo { get; set; }
    public int? ShipmentId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalSurcharge { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public int OrderId { get; set; }
    public Order Order { get; set; }

    public string ItemId { get; set; } // Match type with Item.Id
    public Item Item { get; set; }

    public int Amount { get; set; }
}

public class OrderCreateDto
{
    public int SourceId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequestDate { get; set; }
    public string Reference { get; set; }
    public string ReferenceExtra { get; set; }
    public string OrderStatus { get; set; }
    public string Notes { get; set; }
    public string ShippingNotes { get; set; }
    public string PickingNotes { get; set; }
    public int? WarehouseId { get; set; }
    public int? ShipTo { get; set; }
    public int? BillTo { get; set; }
    public int? ShipmentId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalSurcharge { get; set; }
    public List<OrderItemCreateDto> Items { get; set; } = new();
}

public class OrderItemCreateDto
{
    public string ItemId { get; set; }
    public int Amount { get; set; }
}

public class OrderResponseDto
{
    public int Id { get; set; }
    public int SourceId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequestDate { get; set; }
    public string Reference { get; set; }
    public string ReferenceExtra { get; set; }
    public string OrderStatus { get; set; }
    public string Notes { get; set; }
    public string ShippingNotes { get; set; }
    public string PickingNotes { get; set; }
    public int? WarehouseId { get; set; }
    public int? ShipTo { get; set; }
    public int? BillTo { get; set; }
    public int? ShipmentId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalSurcharge { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<OrderItemResponseDto> Items { get; set; } = new();
}

public class OrderItemResponseDto
{
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } // Maps to Uid in Item

    public int Amount { get; set; }
}

}
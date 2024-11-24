using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models{

public class Order
{
    public int Id { get; set; }
    public int SourceId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequestDate { get; set; }
    public string? Reference { get; set; }
    public string? ExtrReference { get; set; }
    public string? OrderStatus { get; set; }
    public string? Notes { get; set; }
    public string? ShippingNotes { get; set; }
    public string? PickingNotes { get; set; }

    
    public int? WarehouseId { get; set; }
    [JsonIgnore]
    public Warehouse? Warehouse {get; set;}
    public int? ShipTo { get; set; }
    [JsonIgnore]
    public Client? ShipToClient { get; set; }
    public int? BillTo { get; set; }
    [JsonIgnore]
    public Client? BillToClient { get; set; }
    public int? ShipmentId { get; set; }
    [JsonIgnore]
    public Shipment? Shipment { get; set; }  // Navigation property to Shipment
    public decimal TotalAmount { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalSurcharge { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Avoid serializing the OrderItems navigation property
    [JsonIgnore]
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}


public class OrderWithItemsDTO
{
    public int Id { get; set; }
    public int SourceId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequestDate { get; set; }
    public string Reference { get; set; }
    public string? ReferenceExtra { get; set; }
    public string OrderStatus { get; set; }
    public string Notes { get; set; }
    public string ShippingNotes { get; set; }
    public string PickingNotes { get; set; }
    public int? WarehouseId { get; set; } // Nullable
    public int? ShipTo { get; set; } // Nullable
    public int? BillTo { get; set; } // Nullable
    public int? ShipmentId { get; set; } // Nullable
    public decimal TotalAmount { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalSurcharge { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ItemDTO> Items { get; set; }
}


    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Amount { get; set; } // Quantity of the item in the order
    }

public class ItemDTO
{
    public string ItemId { get; set; }
    public int Amount { get; set; }
}





}




// [
//     {
//         "id": 1,
//         "source_id": 33,
//         "order_date": "2019-04-03T11:33:15Z",
//         "request_date": "2019-04-07T11:33:15Z",
//         "reference": "ORD00001",
//         "reference_extra": "Bedreven arm straffen bureau.",
//         "order_status": "Delivered",
//         "notes": "Voedsel vijf vork heel.",
//         "shipping_notes": "Buurman betalen plaats bewolkt.",
//         "picking_notes": "Ademen fijn volgorde scherp aardappel op leren.",
//         "warehouse_id": 18,
//         "ship_to": null,
//         "bill_to": null,
//         "shipment_id": 1,
//         "total_amount": 9905.13,
//         "total_discount": 150.77,
//         "total_tax": 372.72,
//         "total_surcharge": 77.6,
//         "created_at": "2019-04-03T11:33:15Z",
//         "updated_at": "2019-04-05T07:33:15Z",
//         "items": [
//             {
//                 "item_id": "P007435",
//                 "amount": 23
//             },
//           ]
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models{

public class Order{

    public int Id {get; set;}
    public int sourceId {get; set;}
    public DateTime Orderdate {get; set;}
    public DateTime Requestdate {get; set;}
    public string? Reference {get; set;}
    public string? ExtraReference {get;set;} 
    public string? OrderStatus {get; set;}
    public string? Notes {get; set;}
    public string? Shippingnotes {get; set;}
    public string? PickingNotes {get; set;}
    [JsonIgnore]
    public Guid WarehouseId {get; set;}
    [JsonIgnore]
    public Guid ShipTo {get; set;}
    [JsonIgnore]
    public Guid BillTo {get; set;}
    [JsonIgnore]
    public Guid ShipmentId {get; set;}
    public decimal TotalAmount {get;set;}
    public decimal TotalDiscount {get; set;}
    public decimal TotalTax {get; set;}
    public decimal TotalSurcharge {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}
    public List<Items>? Itemlist{get; set;}
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
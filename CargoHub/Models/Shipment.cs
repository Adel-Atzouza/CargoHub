using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;




namespace CargoHub.Models{

public class Shipment : BaseModel{

    public int Id {get; set;}
    public int SourceId {get; set;}
    public DateTime? Orderdate {get; set;}
    public DateTime RequestDate { get; set; }
    public DateTime ShipmentDate { get; set; }
    public string? ShipmentType { get; set; }
    public string? ShipmentStatus { get; set; }
    public string? Notes { get; set; }
    public string? CarrierCode { get; set; }
    public string? CarrierDescription { get; set; }
    public string? ServiceCode { get; set; }
    public string? PaymentType { get; set; }
    public string? TransferMode { get; set; }
    public int TotalPackageCount { get; set; }
    public decimal TotalPackageWeight { get; set; }
    public List<Order>? orders { get; set; }

}


    public class ShipmentDTO
        {
            public int Id { get; set; }
            public DateTime ShipmentDate { get; set; }
            public string ShipmentType { get; set; }
            public string ShipmentStatus { get; set; }
            public string Notes { get; set; }
            public string CarrierCode { get; set; }
            public string CarrierDescription { get; set; }
            public string ServiceCode { get; set; }
            public string PaymentType { get; set; }
            public string TransferMode { get; set; }
            public int TotalPackageCount { get; set; }
            public decimal TotalPackageWeight { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public List<OrderDTO> Orders { get; set; }
        }

    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime RequestDate { get; set; }
        public string Reference { get; set; }
        public string OrderStatus { get; set; }
        public List<ItemDTO> Items { get; set; }
    }
}
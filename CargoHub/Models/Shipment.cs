using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{

    public class Shipment : BaseModel
    {
        public int SourceId { get; set; }
        public DateTime? Orderdate { get; set; }
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
        public double TotalPackageWeight { get; set; }
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
        public double TotalPackageWeight { get; set; }
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
    public class JsonShipment : BaseModel
    {
        [JsonPropertyName("order_id")]
        public int? OrderId { get; set; }

        [JsonPropertyName("source_id")]
        public int? SourceId { get; set; }

        [JsonPropertyName("order_date")]
        public DateTime? OrderDate { get; set; }

        [JsonPropertyName("request_date")]
        public DateTime? RequestDate { get; set; }

        [JsonPropertyName("shipment_date")]
        public DateTime? ShipmentDate { get; set; }

        [JsonPropertyName("shipment_type")]
        public string? ShipmentType { get; set; }

        [JsonPropertyName("shipment_status")]
        public string? ShipmentStatus { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }

        [JsonPropertyName("carrier_code")]
        public string? CarrierCode { get; set; }

        [JsonPropertyName("carrier_description")]
        public string? CarrierDescription { get; set; }

        [JsonPropertyName("service_code")]
        public string? ServiceCode { get; set; }

        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; set; }

        [JsonPropertyName("transfer_mode")]
        public string? TransferMode { get; set; }

        [JsonPropertyName("total_package_count")]
        public int? TotalPackageCount { get; set; }

        [JsonPropertyName("total_package_weight")]
        public double? TotalPackageWeight { get; set; }

        [JsonPropertyName("items")]
        public List<Item>? Items { get; set; }
    }
}
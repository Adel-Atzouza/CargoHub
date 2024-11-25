using System.Text.Json.Serialization;

namespace CargoHub
{
    public class Shipment : BaseModel
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

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

        [JsonIgnore]
        public List<Item>? Items { get; set; }
    }
}

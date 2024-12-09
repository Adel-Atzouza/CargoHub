using System.Text.Json.Serialization;

namespace CargoHub.Dtos
{
    public class ItemDto
    {
        public string? Code { get; set; }
        public string? Description { get; set; }

        [JsonPropertyName("short_description")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("upc_code")]
        public string? UpcCode { get; set; }

        [JsonPropertyName("model_number")]
        public string? ModelNumber { get; set; }

        [JsonPropertyName("commodity_code")]
        public string? CommodityCode { get; set; }

        [JsonPropertyName("item_line")]
        public int ItemLine { get; set; }

        [JsonPropertyName("item_group")]
        public int ItemGroup { get; set; }

        [JsonPropertyName("item_type")]
        public int ItemTypeId { get; set; } // Alleen de ID van ItemType wordt verwacht

        [JsonPropertyName("unit_purchase_quantity")]
        public int UnitPurchaseQuantity { get; set; }

        [JsonPropertyName("unit_order_quantity")]
        public int UnitOrderQuantity { get; set; }

        [JsonPropertyName("pack_order_quantity")]
        public int PackOrderQuantity { get; set; }

        [JsonPropertyName("supplier_id")]
        public int SupplierId { get; set; }

        [JsonPropertyName("supplier_code")]
        public string? SupplierCode { get; set; }

        [JsonPropertyName("supplier_part_number")]
        public string? SupplierPartNumber { get; set; }
    }
}

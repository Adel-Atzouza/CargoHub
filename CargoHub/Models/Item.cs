using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class Item : BaseModel
    {
        [Key]
        public string? Uid { get; set; }
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
        public int ItemLineId { get; set; }
        public ItemLine ItemLine { get; set; }
        public int ItemGroupId { get; set; }
        public ItemGroup ItemGroup { get; set; }
        public int ItemTypeId { get; set; }
        public ItemType ItemType { get; set; }
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
        // Navigation property for many-to-many relationship with Order

    }
}

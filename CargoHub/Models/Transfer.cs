using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public record Transfer
    {
        // Properties
        public Guid Id {get; set;}
        public string? Code {get; set;}
        public string? Name {get; set;}
        public string? Address {get; set;}
        public string? Zip {get; set;}
        public string? City {get; set;}
        public string? Province {get; set;}
        public string? Country {get; set;}
        
        // Contact relation
        [JsonIgnore]
        public Guid ContactId { get; set; }
        public Contact? Contact {get; set;}
        
        // Metadata
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt {get; set;}
        
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt {get; set;}
    }
}


// {
//         "id": 3,
//         "reference": "TR00003",
//         "transfer_from": null,
//         "transfer_to": 9199,
//         "transfer_status": "Completed",
//         "created_at": "2000-03-11T13:11:14Z",
//         "updated_at": "2000-03-12T14:11:14Z",
//         "items": [
//             {
//                 "item_id": "P009557",
//                 "amount": 1
//             }
//         ]
//     },
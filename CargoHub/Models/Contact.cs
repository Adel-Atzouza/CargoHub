using System.Text.Json.Serialization;

namespace CargoHub
{
    public class Contact
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
    }
}
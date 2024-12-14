using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CargoHub.Models
{
    public class MonthlyReportDTO
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalOrders { get; set; }
        public double TotalOrderAmount { get; set; }
        public double AverageOrderProcessingTime { get; set; }
        public double AverageShipmentTransitProcessingTime { get; set; }
        public int TotalShipments { get; set; }
        public List<ShipmentSummaryDTO> ShipmentDetails { get; set; }
    }

    public class ShipmentSummaryDTO
    {
        public int ShipmentId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string ShipmentStatus { get; set; }
        public int TotalOrders { get; set; }
        public double TotalWeight { get; set; }
    }
}
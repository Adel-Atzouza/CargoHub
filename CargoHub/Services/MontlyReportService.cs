using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;
        private readonly PdfReportService _pdfReportService;

        public ReportService(AppDbContext context, PdfReportService pdfReportService)
        {
            _context = context;
            _pdfReportService = pdfReportService;
        }

        public async Task<string> GenerateMonthlyReport(int year, int month)
        {
            // Orders created in the specified month
            var orders = await _context.Orders
                .Where(o => o.CreatedAt.Year == year && o.CreatedAt.Month == month)
                .ToListAsync();

            // Shipments created in the specified month
            var shipments = await _context.Shipments
                .Where(s => s.CreatedAt.Year == year && s.CreatedAt.Month == month)
                .ToListAsync();

            var CalculateAverageOrderProcessingTime =  orders
                    .Where(o => o.OrderDate.HasValue && o.OrderDate > o.CreatedAt)
                    .Select(o => (o.OrderDate.Value - o.CreatedAt).TotalDays)
                    .DefaultIfEmpty(0)
                    .Average();

            var CalculateAverageShipmentTransitProcessingTime =  shipments
                    .Where(s => s.ShipmentDate > s.CreatedAt)
                    .Select(s => (s.ShipmentDate - s.CreatedAt).TotalDays)
                    .DefaultIfEmpty(0)
                    .Average();

            if (!orders.Any() && !shipments.Any())
            {
                throw new InvalidOperationException("No data available for the specified month and year.");
            }

            // Analyze data for the report
            var report = new MonthlyReportDTO
            {
                Year = year,
                Month = month,
                TotalOrders = orders.Count,
                TotalOrderAmount = orders.Sum(o => o.TotalAmount),
                AverageOrderProcessingTime = CalculateAverageOrderProcessingTime,
                TotalShipments = shipments.Count,
                AverageShipmentTransitProcessingTime = CalculateAverageShipmentTransitProcessingTime
            };

            // Generate the PDF and return the file path
            return _pdfReportService.GenerateMonthlyReport(report);
        }
    }
}

using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MonthlyReportDTO> GenerateMonthlyReport(int year, int month)
        {
            // Orders verwerkt in de opgegeven maand
            var orders = await _context.Orders
                .Where(o => o.CreatedAt.Year == year && o.CreatedAt.Month == month)
                .ToListAsync();

            Console.WriteLine($"Filtered Orders: {orders.Count}");

            // Shipments verzonden in de opgegeven maand
            var shipments = await _context.Shipments
                .Where(s => s.CreatedAt.Year == year && s.CreatedAt.Month == month)
                .ToListAsync();

            Console.WriteLine($"Filtered Shipments: {shipments.Count}");

            // Analyseer orderverwerking
            var totalOrders = orders.Count;
            var totalShipments = shipments.Count;
            var totalOrderAmount = orders.Sum(o => o.TotalAmount);

            // Gemiddelde tijd tussen aanmaak en afleveren van orders
            var averageOrderDeliveryTime = orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate > o.CreatedAt)
                .Select(o => (o.OrderDate.Value - o.CreatedAt).TotalDays)
                .DefaultIfEmpty(0)
                .Average();

            // Gemiddelde tijd tussen aanmaak en transit van shipments
            var averageShipmentTransitTime = shipments
                .Where(s => s.ShipmentDate > s.CreatedAt)
                .Select(s => (s.ShipmentDate - s.CreatedAt).TotalDays)
                .DefaultIfEmpty(0)
                .Average();

            return new MonthlyReportDTO
            {
                Year = year,
                Month = month,
                TotalOrders = totalOrders,
                TotalOrderAmount = totalOrderAmount,
                AverageOrderProcessingTime = averageOrderDeliveryTime,
                TotalShipments = totalShipments,
                AverageShipmentTransitProcessingTime = averageShipmentTransitTime,
                ShipmentDetails = shipments.Select(s => new ShipmentSummaryDTO
                {
                    ShipmentId = s.Id,
                    ShipmentDate = s.ShipmentDate,
                    ShipmentStatus = s.ShipmentStatus,
                    TotalOrders = s.orders?.Count ?? 0,
                    TotalWeight = s.TotalPackageWeight
                }).ToList()
            };
        }
    }
}
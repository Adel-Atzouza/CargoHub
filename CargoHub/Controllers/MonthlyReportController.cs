using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using CargoHub.Services;

namespace CargoHub.Controllers
{
    [Route("api/v1/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("monthly-report")]
        public async Task<IActionResult> GenerateMonthlyReport(int year, int month)
        {
            try
            {
                // Genereer het rapport
                var report = await _reportService.GenerateMonthlyReport(year, month);

                // Stel het pad in naar de Reports-folder
                string reportsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
                string filePath = Path.Combine(reportsFolder, "MonthlyReport.pdf");

                // Controleer of het bestand bestaat
                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, "application/pdf", $"MonthlyReport.pdf");
                }
                else
                {
                    return NotFound("Report not found.");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Handle no data case
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating report: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

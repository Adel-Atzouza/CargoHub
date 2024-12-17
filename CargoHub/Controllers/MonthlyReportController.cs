using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    var report = await _reportService.GenerateMonthlyReport(year, month);

                    string filePath = "MonthlyReport.pdf";
                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                        return File(fileBytes, "application/pdf", filePath);
                    }
                    else
                    {
                        return NotFound("Report not found.");
                    }
                }
                catch (Exception ex)
                {
                    // Log de fout (bijvoorbeeld met een logging framework zoals Serilog)
                    Console.WriteLine($"Error generating report: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
        }
    }
}

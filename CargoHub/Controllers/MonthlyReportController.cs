using Microsoft.AspNetCore.Mvc;
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
            if (year <= 0 || month <= 0 || month > 12)
            {
                return BadRequest(new { message = "Invalid year or month. Please provide valid values." });
            }

            // Generate the report and get the file path
            string filePath = await _reportService.GenerateMonthlyReport(year, month);

            // Serve the file if it exists
            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                string fileName = Path.GetFileName(filePath);

                return Ok();
            }
            else
            {
                return NotFound(new { message = "Report not found." });
            }
        }
    }
}

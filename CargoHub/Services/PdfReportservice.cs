using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using System;
using CargoHub.Models;

namespace CargoHub.Services
{
    public class PdfReportService
    {
        private readonly string _reportsFolder;

        public PdfReportService()
        {
            // Set the Reports directory path and ensure it exists
            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            _reportsFolder = Path.Combine(projectRoot, "Reports");
        }

        public string GenerateMonthlyReport(MonthlyReportDTO report)
        {
            // Generate the file path dynamically
            string fileName = $"MonthlyReport_{report.Year}_{report.Month:D2}.pdf";
            string filePath = Path.Combine(_reportsFolder, fileName);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Add content to the PDF
                document.Add(new Paragraph($"Monthly Report for {report.Month:D2}/{report.Year}"));

                if (report.TotalOrders == 0 && report.TotalShipments == 0)
                {
                    document.Add(new Paragraph("No data available for this period."));
                }
                else
                {
                    document.Add(new Paragraph($"Total Orders: {report.TotalOrders}"));
                    document.Add(new Paragraph($"Total Order Amount: {report.TotalOrderAmount:C}"));
                    document.Add(new Paragraph($"Average Order Processing Time: {report.AverageOrderProcessingTime} days"));
                    document.Add(new Paragraph($"Total Shipments: {report.TotalShipments}"));
                    document.Add(new Paragraph($"Average Shipment Transit Time: {report.AverageShipmentTransitProcessingTime} days"));
                }

                document.Close();
            }

            return filePath;
        }
    }
}

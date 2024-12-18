using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CargoHub.Models;



namespace CargoHub.Services{
public class PdfReportService
{
public void GenerateMonthlyReport(MonthlyReportDTO report)
{
    // Stel het pad in voor de Reports-map
    string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
    string reportsFolder = Path.Combine(projectRoot, "Reports");

    // Zorg dat de Reports-map bestaat
    if (!Directory.Exists(reportsFolder))
    {
        Directory.CreateDirectory(reportsFolder);
    }

    // Stel het volledige pad in voor het PDF-bestand
    string dest = Path.Combine(reportsFolder, "MonthlyReport.pdf");

    try
    {
        Console.WriteLine($"Saving PDF to: {dest}");

        using (FileStream fs = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            PdfWriter writer = new PdfWriter(fs);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            if (report.TotalOrders == 0 && report.TotalShipments == 0)
            {
                document.Add(new Paragraph($"Monthly Report for {report.Month}/{report.Year}"));
                document.Add(new Paragraph("No data available for this period."));
            }
            else
            {
                document.Add(new Paragraph($"Monthly Report for {report.Month}/{report.Year}"));
                document.Add(new Paragraph($"Total Orders: {report.TotalOrders}"));
                document.Add(new Paragraph($"Total Order Amount: {report.TotalOrderAmount:C}"));
                document.Add(new Paragraph($"Average Order Processing Time: {report.AverageOrderProcessingTime} days"));
                document.Add(new Paragraph($"Total Shipments: {report.TotalShipments}"));
                document.Add(new Paragraph($"Average Shipment Transit Time: {report.AverageShipmentTransitProcessingTime} days"));
            }

            document.Close();
        }

        Console.WriteLine("PDF generated successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error generating PDF: {ex.Message}");
        throw;
    }
}

}
}
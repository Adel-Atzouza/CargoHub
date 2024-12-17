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
        string dest = "MonthlyReport.pdf"; // De naam en locatie van het PDF-bestand
        try
        {
            using (FileStream fs = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph($"Monthly Report for {report.Month}/{report.Year}"));
                document.Add(new Paragraph($"Total Orders: {report.TotalOrders}"));
                document.Add(new Paragraph($"Total Order Amount: {report.TotalOrderAmount:C}"));
                document.Add(new Paragraph($"Average Order Processing Time: {report.AverageOrderProcessingTime} days"));
                document.Add(new Paragraph($"Total Shipments: {report.TotalShipments}"));
                document.Add(new Paragraph($"Average Shipment Transit Time: {report.AverageShipmentTransitProcessingTime} days"));

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
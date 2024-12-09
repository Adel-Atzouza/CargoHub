using CargoHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Text.Json;

namespace CargoHub.Services
{
    public class MigrationsService
    {
        AppDbContext _context;
        public MigrationsService(AppDbContext context)
        {
            _context = context;
        }

        public List<string> ReadDataFolder(string Folder)
        {
            var stopwatch = Stopwatch.StartNew(); // Start the stopwatch

            string Path = $"../{Folder}";
            var files = Directory.GetFiles(Path);
            List<string> FilesNames = new();

            foreach (var file in files)
            {
                if (file.Contains("clients"))
                {
                    TransferData<Client>(file);
                }
                else if (file.Contains("item_groups"))
                {
                    TransferData<ItemGroup>(file);
                }
                else if (file.Contains("inventories"))
                {
                    TransferData<Inventory>(file);
                }
                else if (file.Contains("item_lines"))
                {
                    TransferData<ItemLine>(file);
                }
                else if (file.Contains("item_types"))
                {
                    TransferData<ItemType>(file);
                }
                else if (file.Contains("items"))
                {
                    TransferData<Item>(file);
                }
                else
                {
                    Console.WriteLine("This file is not included in the transfer files");
                }
            }

            stopwatch.Stop(); // Stop the stopwatch
            Console.WriteLine($"Data transfer took: {stopwatch.ElapsedMilliseconds} ms");

            return FilesNames;
        }

        private void TransferData<T>(string DataFile) where T : BaseModel
        {
            var stopwatch = Stopwatch.StartNew(); // Start the stopwatch for this method

            var options = new JsonSerializerOptions();
            options.Converters.Add(new CustomDateTimeConverter());

            try
            {
                List<T>? Models = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(DataFile), options);
                if (Models != null && Models.Count > 0)
                {
                    if (Models[0].Id == 0)
                    {
                        Models.ForEach(m => m.Id += 1);
                    }
                    _context.AddRange(Models);
                    int RowsChanged = _context.SaveChanges();
                    Console.WriteLine($"Rows inserted: {RowsChanged}");
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }

            stopwatch.Stop(); // Stop the stopwatch for this method
            Console.WriteLine($"Data transfer for file {DataFile} took: {stopwatch.ElapsedMilliseconds} ms");
        }

        private void LogTransfer(bool Success, string File, int RowsChanged = 0)
        {
            throw new NotImplementedException();
        }

    }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace CargoHub
{
    public class DataMigration
    {
        private AppDbContext _context { get; set; }
        public DataMigration(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<string>> MigrateData(string folder = "../Data")
        {
            var processedFiles = new List<string>();
            var jsonFiles = Directory.GetFiles(folder);

            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    await ProcessFile(jsonFile);
                    processedFiles.Add(jsonFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {jsonFile}: {ex.Message}");
                }
            }
            return processedFiles;
        }

        public async Task ProcessFile(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            var options = new JsonSerializerOptions
            {
                Converters = { new DateTimeConverter() }
            };

            switch (fileName)
            {
                case "clients":
                    try
                    {
                        var clients = JsonSerializer.Deserialize<List<Client>>(await File.ReadAllTextAsync(filePath), options);
                        await _context.Clients.AddRangeAsync(clients);
                        await _context.SaveChangesAsync();

                        return;
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
                    {
                        Console.WriteLine($"{e.InnerException}");
                        break;
                    }




                // case "inventories":
                //     var inventories = JsonSerializer.Deserialize<List<Inventory>>(await File.ReadAllTextAsync(filePath), options);
                //     if (inventories.Count > 0)
                //     {
                //         _context.Inventories.AddRange(inventories);
                //         await _context.SaveChangesAsync();
                //         break;
                //     }
                //     return;

                //                 case "item_groups":
                //                     try
                //                     {
                //                         var ItemGroups = JsonSerializer.Deserialize<List<ItemGroup>>(await File.ReadAllTextAsync(filePath), options);
                //                         await _context.ItemGroups.AddRangeAsync(ItemGroups);
                //                         await _context.SaveChangesAsync();
                //                         return;
                //                     }
                //                     catch (Exception e)
                //                     {
                //                         Console.WriteLine($"{e.Message}");
                //                         if (e.InnerException != null) 
                //                         {
                //                             Console.WriteLine("InnerException: " + e.InnerException);
                //                         }
                //                         break;
                //                     }

                //                 case "item_lines":
                //                     var ItemLines = JsonSerializer.Deserialize<List<ItemLine>>(await File.ReadAllTextAsync(filePath), options);
                //                     try
                //                     {
                // _context.ItemLines.AddRange(ItemLines);
                //                         await _context.SaveChangesAsync();
                //                         break;
                //                     }
                //                     catch (Exception e)
                //                     {
                //                         Console.WriteLine($"{e.Message}");
                //                         if (e.InnerException != null) 
                //                         {
                //                             Console.WriteLine("InnerException: " + e.InnerException);
                //                         }
                //                         break;
                //                     }


                // case "locations":
                //     var Locations = JsonSerializer.Deserialize<List<Client>>(await File.ReadAllTextAsync(filePath), options);
                //     if (Locations.Count > 0)
                //     {
                //         _context.Locations.AddRange(Locations);
                //         await _context.SaveChangesAsync();
                //         break;
                //     }
                //     return;

                default:
                    Console.WriteLine($"No matching handler for {fileName}. Skipping...");
                    return;
            }
        }


        public void LogMigration(string filePath)
        {
            var logPath = "../LogData/migration.log";

            if (!Directory.Exists("../LogData"))
            {
                Directory.CreateDirectory("../LogData");
            }

            File.AppendAllText(logPath, $"{filePath} migrated at {DateTime.Now}{Environment.NewLine}");
        }
    }
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string CustomFormat = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString()!, CustomFormat, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(CustomFormat));
        }
    }
}

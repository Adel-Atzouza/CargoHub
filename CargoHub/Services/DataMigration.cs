using System.Text.Json;
using System.Text.Json.Serialization;

namespace CargoHub
{
    public class DataMigration
    {
        private readonly AppDbContext _context;

        public DataMigration(AppDbContext dbContext)
        {
            _context = dbContext;
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

        private async Task ProcessFile(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            var options = new JsonSerializerOptions
            {
                Converters = { new DateTimeConverter() }
            };

            switch (fileName)
            {
                case "clients":

                    var clients = JsonSerializer.Deserialize<List<Client>>(await File.ReadAllTextAsync(filePath), options);
                    if (clients.Count > 0)
                    {
                        _context.Clients.AddRange(clients);
                        await _context.SaveChangesAsync();
                        break;
                    }
                    return;
                    

                // case "inventories":
                //     var inventories = JsonSerializer.Deserialize<List<Inventory>>(await File.ReadAllTextAsync(filePath), options);
                //     if (inventories.Count > 0)
                //     {
                //         _context.Inventories.AddRange(inventories);
                //         await _context.SaveChangesAsync();
                //         break;
                //     }
                //     return;

                case "item_groups":
                    var ItemGroups = JsonSerializer.Deserialize<List<ItemGroup>>(await File.ReadAllTextAsync(filePath), options);
                    if (ItemGroups.Count > 0)
                    {
                        _context.ItemGroups.AddRange(ItemGroups);
                        await _context.SaveChangesAsync();
                        break;
                    }
                    return;

                case "item_lines":
                    var ItemLines = JsonSerializer.Deserialize<List<ItemLine>>(await File.ReadAllTextAsync(filePath), options);
                    if (ItemLines.Count > 0)
                    {
                        _context.ItemLines.AddRange(ItemLines);
                        await _context.SaveChangesAsync();
                        break;
                    }
                    return;

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
                    break;
            }
        }

        // private async Task<List<T>> DeserializeJson<T>(string filePath)
        // {
        //     try
        //     {
        //         var jsonData = await File.ReadAllTextAsync(filePath);

        //         var options = new JsonSerializerOptions
        //         {
        //             Converters = { new DateTimeConverter() }
        //         };

        //         return JsonSerializer.Deserialize<List<T>>(jsonData, options)!;
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error deserializing {filePath}: {ex.Message}");
        //         return new List<T>();
        //     }
        // }
        private async Task SaveToDatabase<T>(List<T> entities) where T : class
        {
            if (entities.Count == 0) return;

            _context.AddRange(entities);
            int changed_rows = await _context.SaveChangesAsync();
            Console.WriteLine(changed_rows);
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

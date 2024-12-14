using CargoHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

            string Path = $"../{Folder}";
            var files = Directory.GetFiles(Path);
            List<string> FilesNames = new();
            bool WarehouseInserted = false;

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
                else if (file.Contains("warehouses"))
                {
                    TransferData<Warehouse>(file);
                    WarehouseInserted = true;
                }

                else if (file.Contains("locations"))
                {
                    if (WarehouseInserted)
                    {
                        TransferData<Location>(file);

                    }
                    continue;
                }
                else if (file.Contains("suppliers"))
                {
                    TransferData<Supplier>(file);
                }
                else if (file.Contains("transfers"))
                {
                    TransferData<Transfer>(file);

                }
                else if (file.Contains("shipments"))
                {
                    TransferData<Shipment>(file);

                }


                else
                {
                    Console.WriteLine($"The file {file} is not included in the transfer files");
                }
            }


            return FilesNames;
        }

        private void TransferData<T>(string DataFile) where T : BaseModel
        {

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
        }

        private void LogTransfer(bool Success, string File, int RowsChanged = 0)
        {
            throw new NotImplementedException();
        }

    }
}

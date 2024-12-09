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

            foreach (var file in files)
            {
                if (file.Contains("item_groups"))
                {
                    try
                    {
                        TransferData<ItemGroup>(file);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

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
                if (Models[0].Id == 0)
                {
                    Models.ForEach(m => m.Id += 1);
                }
                _context.AddRange(Models);
                int RowsChanged = _context.SaveChanges();
                Console.WriteLine(RowsChanged);

            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing : {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                if (ex.InnerException != null)
                { Console.WriteLine($"Inner Exception: {ex.InnerException.Message}"); }
            }

        }

        private void LogTransfer(bool Success, string File, int RowsChanged = 0)
        {
            throw new NotImplementedException();
        }



    }
}
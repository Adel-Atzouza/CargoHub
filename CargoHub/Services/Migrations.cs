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

        public void TransferData<T>(string DataFile) where T : BaseModel
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CustomDateTimeConverter());
            try
            {
                List<T>? Models = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(DataFile), options);
                foreach (var model in Models)
                {
                    Console.WriteLine($"===={model.Id}====");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing : {ex.Message}");
            }



        }


    }
}
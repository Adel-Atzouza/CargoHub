using CargoHub.Models;
using System.IO;

namespace CargoHub.Services
{
    public class MigrationsService
    {
        AppDbContext _context;
        public MigrationsService(AppDbContext context)
        {
            _context = context;
        }

        public List<string> ReadData(string Folder)
        {
            string Path = $"../{Folder}";
            var files = Directory.GetFiles(Path);
            List<string> FileName = new();

            foreach (var file in files)
            {
                FileName.Add(System.IO.Path.GetFileName(file));

            }
            return FileName;
        }
    }
}
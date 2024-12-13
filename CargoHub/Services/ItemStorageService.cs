using CargoHub.Models;
using Microsoft.EntityFrameworkCore;


namespace CargoHub.Services
{
    public class ItemStorageService(AppDbContext appDbContext) : BaseStorageService(appDbContext)
    {
        private new readonly AppDbContext appDbContext = appDbContext;

        private static string IdToUid(int id)
        {
            return $"P{id.ToString().PadLeft(6, '0')}";
        }

        public int? UidToId(string uid)
        {
            return int.Parse(uid[1..]);
        }

        public async Task<string?> AddRowUid<T>(T row) where T : Item
        {
            row.Id = appDbContext.Set<T>().Any() ? appDbContext.Set<T>().Max(_ => _.Id) + 1 : 1;
            row.Uid = IdToUid(row.Id);

            if (await base.AddRow(row) is null)
                return null;
            
            return row.Uid;
        }
    }
}
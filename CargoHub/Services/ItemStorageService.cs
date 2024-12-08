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

        private static int? UidToId(string uid)
        {
            return int.Parse(uid[1..]);
        }

        public async Task<T?> GetRow<T>(string uid) where T : Item
        {
            int? id = UidToId(uid);
            if (id is null) return null;
            return await base.GetRow<T>((int)id);
        }

        public async Task<string?> AddRowUid<T>(T row) where T : Item
        {
            row.Id = appDbContext.Set<T>().Any() ? appDbContext.Set<T>().Max(_ => _.Id) + 1 : 1;
            row.Uid = IdToUid(row.Id);

            if (await base.AddRow(row) is null)
                return null;
            
            return row.Uid;
        }

        public async Task<bool?> UpdateRow<T>(string uid, T row) where T : Item
        {
            int? id = UidToId(uid);
            if (id is null) return null;
            return await base.UpdateRow<T>((int)id, row, ["Uid"]);
        }

        public async Task<bool> DeleteRow<T>(string uid) where T : Item
        {
            int? id = UidToId(uid);
            if (id is null) return false;
            return await base.DeleteRow<T>((int)id);
        }
    }
}
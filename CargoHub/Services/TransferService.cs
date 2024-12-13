using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub
{
    public class TransferService
    {
        private readonly AppDbContext appDbContext;
        public TransferService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Transfer?> GetTransfer(int id)
        {
            return await appDbContext.Transfers
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<Transfer>> GetAllTransfers()
        {
            return await appDbContext.Transfers.ToListAsync();
        }

        // public async Task<int?> PostTransfer(Transfer Transfer)
        // {
        //     var transfer = Transfer with { Id = appDbContext.Transfers.Any() ? appDbContext.Transfers.Max(w => w.Id) + 1 : 1 };
        //     await appDbContext.Transfers.AddAsync(transfer);
        //     int n = await appDbContext.SaveChangesAsync();
        //     return n > 0 ? transfer.Id : null;
        // }

        public async Task<int?> PutTransfer(int id, Transfer Transfer)
        {
            var transfer = await appDbContext.Transfers.FindAsync(id);
            if (transfer == null) return null;

            appDbContext.Entry(transfer).CurrentValues.SetValues(Transfer);
            transfer.UpdatedAt = DateTime.Now;
            transfer.TransferStatus = "Scheduled";

            int n = await appDbContext.SaveChangesAsync();
            return n > 0 ? transfer.Id : null;
        }

        public async Task<bool> DeleteTransfer(int id)
        {
            var transfer = await appDbContext.Transfers.FindAsync(id);
            if (transfer == null) return false;
            appDbContext.Transfers.Remove(transfer);
            int n = await appDbContext.SaveChangesAsync();
            return n > 0;
        }
    }
}
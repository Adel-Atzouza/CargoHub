namespace CargoHub.Services
{
    public class BaseStorage(AppDbContext appDbContext)
    {
        public virtual Task<T?> GetRow<T>(int id)
        {
            return await appDbContext.Set<T>().FirstOrDefaultAsync(_ => _.Id == id);
        }

        public virtual async Task<List<T>> GetAllRows<T>(int pageNumber = 1, int pageSize = 100)
        {
            // Test what happens when the page number is out of bounds
            return await appDbContext.Set<T>()
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();
        }
        public virtual async Task<int> AddRow<T>(T row)
        {
            bool alreadyExists = await appDbContext.Set<T>().ContainsAsync(row);
            if (row == null || alreadyExists) return -1;

            appDbContext.Set<T>().Add(row);
            await appDbContext.SaveChangesAsync();

            var entry = appDbContext.Entry(row);
            return (int)entry.Property("Id").CurrentValue;
        }
        
        public virtual async Task<bool> UpdateRow<T>(T row, int id)
        {
            string[] excludedProperties = { "CreatedAt" };

            var found = await appDbContext.Set<T>().FirstOrDefaultAsync(_ => _.Id == id);
            if (row == null || found == null) return false;

            var entry = appDbContext.Entry(found);
            entry.CurrentValues.SetValues(row);

            foreach (var property in excludedProperties)
            {
                entry.Property(property).IsModified = false;
            }

            if (entry.Property("UpdatedAt") != null)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
            }

            await appDbContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteWarehouse<T>(int id)
        {
            var row = await appDbContext.Set<T>().FirstOrDefaultAsync(_ => _.Id == id);
            if (row == null) return false;
            appDbContext.Set<T>().Remove(row);
            int n = await appDbContext.SaveChangesAsync();
            return n > 0;
        }
    }
}
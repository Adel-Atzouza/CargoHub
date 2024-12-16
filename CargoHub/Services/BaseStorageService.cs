using Microsoft.EntityFrameworkCore;

namespace CargoHub.Services
{
    public class BaseStorageService(AppDbContext appDbContext)
    {
        protected readonly AppDbContext appDbContext = appDbContext;

        public virtual async Task<T?> GetRow<T>(int id) where T : BaseModel
        {
            return await appDbContext.Set<T>().FirstOrDefaultAsync(_ => _.Id == id);
        }

        public virtual async Task<List<T>> GetAllRows<T>(int pageNumber = 1, int pageSize = 100) where T : BaseModel
        {
            return await appDbContext.Set<T>()
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();
        }

        public virtual async Task<int?> AddRow<T>(T row) where T : BaseModel
        {            
            // bool alreadyExists = await appDbContext.Set<T>().ContainsAsync(row);
            // if (row == null || alreadyExists) return null;
            if (row == null) return null;

            row.Id = appDbContext.Set<T>().Any() ? appDbContext.Set<T>().Max(_ => _.Id) + 1 : 1;

            await appDbContext.Set<T>().AddAsync(row);
            await appDbContext.SaveChangesAsync();

            var entry = appDbContext.Entry(row);
            return (int?)entry.Property("Id").CurrentValue;
        }
        
        public virtual async Task<bool> UpdateRow<T>(int id, T row, string[]? excludedProperties = null) where T : BaseModel
        {
            excludedProperties ??= [];
            excludedProperties = [.. excludedProperties, "Id", "Uid", "CreatedAt", "UpdatedAt"];
            
            var found = await appDbContext.Set<T>().FirstOrDefaultAsync(_ => _.Id == id);
            if (row == null || found == null) return false;

            if (row.Id != id)
            {
                return false;
            }

            var entry = appDbContext.Entry(found);
            entry.CurrentValues.SetValues(row);

            foreach (var property in excludedProperties)
            {
                if (entry.OriginalValues.Properties.Any(p => p.Name == property))
                {
                    entry.Property(property).IsModified = false;
                }
            }

            entry.Property("UpdatedAt").CurrentValue = DateTime.Now;

            await appDbContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteRow<T>(int id) where T : BaseModel
        {
            var row = await appDbContext.Set<T>().FirstOrDefaultAsync(_ => _.Id == id);
            if (row == null) return false;
            appDbContext.Set<T>().Remove(row);
            int n = await appDbContext.SaveChangesAsync();
            return n > 0;
        }
    }
}
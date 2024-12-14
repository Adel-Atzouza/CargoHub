using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
namespace CargoHub.Services
{
    public class ClassificationService
    {
        private readonly AppDbContext _context;
        public ClassificationService(AppDbContext context)
        {
            _context = context;
        }

        // public async Task<List<Classification>> GetAllClassifications()
        // {
        // List<Classification> classifications = await _context.Classifications.ToListAsync();
        //     return classifications;

        // }
    }
}
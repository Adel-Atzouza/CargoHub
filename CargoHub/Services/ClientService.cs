using CargoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoHub.Services
{
    public class ClientService
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        // Get a list of clients, with an optional filter based on a minimum ID
        public async Task<List<Client>> GetClients(int id = 0)
        {
            return await _context.Clients
                .Where(client => client.Id >= id)
                .OrderBy(client => client.Id)
                .Take(100)
                .ToListAsync();
        }

        // Get a single client by ID
        public async Task<Client> GetClient(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(client => client.Id == id);
        }

        // Add a new client
        public async Task<string> AddClient(Client client)
        {
            client.CreatedAt = DateTime.UtcNow;
            client.UpdatedAt = DateTime.UtcNow;
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return "Client added successfully.";
        }

        // Update an existing client by ID
        public async Task<string> UpdateClient(int id, Client updatedClient)
        {
            var existingClient = await _context.Clients.FindAsync(id);
            if (existingClient == null)
            {
                return "Error: Client not found.";
            }

            updatedClient.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingClient).CurrentValues.SetValues(updatedClient);
            await _context.SaveChangesAsync();
            return "Client updated successfully.";
        }

        // Delete a client by ID
        public async Task<string> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return "Error: Client not found.";
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return "Client deleted successfully.";
        }
    }
}

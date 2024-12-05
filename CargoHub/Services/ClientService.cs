using CargoHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class ClientService
    {
        private readonly AppDbContext appDbContext;

        public ClientService(AppDbContext context)
        {
            appDbContext = context;
        }

        // Get a list of clients, with an optional filter based on a minimum ID
        public async Task<List<Client>> GetClients(int id = 0)
        {
            return await appDbContext.Clients
                .Where(client => client.Id >= id)
                .OrderBy(client => client.Id)
                .Take(100)
                .ToListAsync();
        }

        // Get a single client by ID
        public async Task<Client> GetClient(int id)
        {
            return await appDbContext.Clients.FirstOrDefaultAsync(client => client.Id == id);
        }

        // Add a new client
        public async Task<string> AddClient(Client client)
        {
            client.CreatedAt = DateTime.UtcNow;
            client.UpdatedAt = DateTime.UtcNow;
            appDbContext.Clients.Add(client);
            await appDbContext.SaveChangesAsync();
            return "Client added successfully.";
        }

        // Update an existing client by ID
        public async Task<string> UpdateClient(int id, Client updatedClient)
        {
            var existingClient = await appDbContext.Clients.FirstOrDefaultAsync(_ => _.Id == id);
            if (existingClient == null)
            {
                return "Error: Client not found.";
            }

            updatedClient.UpdatedAt = DateTime.UtcNow;
            appDbContext.Entry(existingClient).CurrentValues.SetValues(updatedClient);
            await appDbContext.SaveChangesAsync();
            return "Client updated successfully.";
        }

        // Delete a client by ID
        public async Task<string> DeleteClient(int id)
        {
            var client = await appDbContext.Clients.FirstOrDefaultAsync(_ => _.Id == id);
            if (client == null)
            {
                return "Error: Client not found.";
            }

            appDbContext.Clients.Remove(client);
            await appDbContext.SaveChangesAsync();
            return "Client deleted successfully.";
        }
    }
}

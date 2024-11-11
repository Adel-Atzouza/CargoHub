using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoHub.Controllers
{
    [Route("api/v1/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: api/Client/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            var client = await _clientService.GetClient(id);
            if (client == null)
                return NotFound("Client not found.");

            return Ok(client);
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult<List<Client>>> GetClients([FromQuery] int id = 0)
        {
            var clients = await _clientService.GetClients(id);
            return Ok(clients);
        }

        // POST: api/Client
        [HttpPost]
        public async Task<ActionResult<string>> CreateClient([FromBody] Client client)
        {
            var result = await _clientService.AddClient(client);
            return Ok(result);
        }

        // PUT: api/Client/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateClient(int id, [FromBody] Client client)
        {
            var result = await _clientService.UpdateClient(id, client);
            if (result == "Error: Client not found.")
                return NotFound(result);

            return Ok(result);
        }

        // DELETE: api/Client/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteClient(int id)
        {
            var result = await _clientService.DeleteClient(id);
            if (result == "Error: Client not found.")
                return NotFound(result);

            return Ok(result);
        }
    }
}

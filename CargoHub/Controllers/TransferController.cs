using CargoHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoHub.Controllers
{
    [Route("api/v1/transfers")]
    public class TransferController(AppDbContext appDbContext, TransferService storage) : Controller
    {
        AppDbContext appDbContext = appDbContext;
        TransferService storage = storage;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransfer(int id)
        {               
            var transfer = await storage.GetTransfer((int)id);
            
            if (transfer == null)
                return NotFound("Transfer doesn't exist with id: " + id);
            
            return Ok(transfer);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllTransfers([FromQuery] int page = 0)
        {
            var transfers = await storage.GetAllTransfers();
            var paginatedTransfers = transfers.Skip(page * 100).Take(100).ToList();
            return Ok(paginatedTransfers);
        }

        [HttpPost()]
        public async Task<IActionResult> PostTransfer([FromBody] Transfer transfer)
        {
            if (transfer == null)
            return BadRequest("Transfer cannot be null");

            var createdTransferId = await storage.PostTransfer(transfer);
            return CreatedAtAction(nameof(GetTransfer), new { id = createdTransferId }, createdTransferId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransfer(int id, [FromBody] Transfer transfer)
        {
            if (transfer == null)
                return BadRequest("Transfer cannot be null");

            var updatedTransfer = await storage.PutTransfer(id, transfer);
            if (updatedTransfer == null)
                return NotFound("Transfer doesn't exist with id: " + id);

            return Ok(updatedTransfer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransfer(int id)
        {
            var transfer = await storage.GetTransfer(id);
            if (transfer == null)
                return NotFound("Transfer doesn't exist with id: " + id);

            return Ok(await storage.DeleteTransfer(id));
        }
    }
}
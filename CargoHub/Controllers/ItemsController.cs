using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]    
    public abstract class ItemsController(ItemStorageService storage) : Controller
    {
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetRow(int id)
        {               
            var row = await storage.GetRow<Item>(id);
            
            if (row == null)
                return NotFound("Warehouse doesn't exist with id: " + id);
            
            return Ok(row);
        }

        [HttpGet()]
        public virtual async Task<IActionResult> GetAllRows([FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            var row = await storage.GetAllRows<Item>(page, pageSize);
            return Ok(row);
        }

        [HttpPost()]
        public virtual async Task<IActionResult> PostRow([FromBody] Item row)
        {
            if (row == null)
                return BadRequest("Warehouse cannot be null");

            var rowId = await storage.AddRowUid(row);
            return CreatedAtAction(nameof(GetRow), new { id = rowId }, rowId);
        }

        [HttpPut]
        public virtual async Task<IActionResult> PutRow([FromQuery] int id, [FromBody] Item row)
        {
            if (row == null)
                return BadRequest("Warehouse cannot be null");

            var updatedRow = await storage.UpdateRow(id, row);
            if (!updatedRow)
                return NotFound("Warehouse doesn't exist with id: " + id);

            return Ok(updatedRow);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteRow(int id)
        {
            var row = await storage.DeleteRow<Item>(id);
            if (!row)
                return NotFound("Warehouse doesn't exist with id: " + id);

            return Ok(await storage.DeleteRow<Item>(id));
        }
    }
}

using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    public abstract class BaseController<T>(BaseStorageService storage) : Controller where T : BaseModel
    {
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetRow(int id)
        {               
            var row = await storage.GetRow<T>(id);
            
            if (row == null)
                return NotFound("Warehouse doesn't exist with id: " + id);
            
            return Ok(row);
        }

        [HttpGet()]
        public virtual async Task<IActionResult> GetAllRows([FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            var row = await storage.GetAllRows<T>(page, pageSize);
            return Ok(row);
        }

        [HttpPost()]
        public virtual async Task<IActionResult> PostRow([FromBody] T row)
        {
            if (row == null)
                return BadRequest("Warehouse cannot be null");

            var rowId = await storage.AddRow(row);
            return CreatedAtAction(nameof(GetRow), new { id = rowId }, rowId);
        }

        [HttpPut]
        public virtual async Task<IActionResult> PutRow([FromQuery] int id, [FromBody] T row)
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
            var row = await storage.DeleteRow<T>(id);
            if (!row)
                return NotFound("Warehouse doesn't exist with id: " + id);

            return Ok(await storage.DeleteRow<T>(id));
        }
    }
}
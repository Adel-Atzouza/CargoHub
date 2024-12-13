using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    public abstract class BaseController<T>(BaseStorageService storage) : Controller where T : BaseModel
    {
        private static int? TryConvertToInt(string id)
        {
            if (int.TryParse(id, out int result))
                return result; 

            if (!string.IsNullOrWhiteSpace(id) && id.StartsWith('P'))
                if (int.TryParse(id[1..], out result))
                    return result;

            return null; 
        }


        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetRow(string id)
        {
            var _id = TryConvertToInt(id);
            if (_id is null) return BadRequest("Id is not valid: " + id);
            
            var row = await storage.GetRow<T>((int)_id);
            
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

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> PutRow(string id, [FromBody] T row)
        {
            var _id = TryConvertToInt(id);
            if (_id is null) return BadRequest("Id is not valid: " + id);
            
            if (row == null)
                return BadRequest("Warehouse cannot be null");

            var updatedRow = await storage.UpdateRow((int)_id, row);
            if (!updatedRow)
                return NotFound("Warehouse doesn't exist with id: " + id);

            return Ok(updatedRow);
        }


        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteRow(string id)
        {
            var _id = TryConvertToInt(id);
            if (_id is null) return BadRequest("Id is not valid: " + id);

            var row = await storage.DeleteRow<T>((int)_id);
            if (!row)
                return NotFound("Warehouse doesn't exist with id: " + id);

            return Ok("Row deleted");
        }
    }
}
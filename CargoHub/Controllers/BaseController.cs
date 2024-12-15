using System.Globalization;
using CargoHub.Services;
using CargoHub.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    public abstract class BaseController<T>(BaseStorageService storage, ErrorHandler error) : ControllerBase where T : BaseModel
    {
        protected readonly string Name = typeof(T).Name;
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
            if (_id is null) return error.IdInvalid(Name, id);
            
            var row = await storage.GetRow<T>((int)_id);
            
            if (row == null)
                return error.IdNotFound(Name, id);
            
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
                return error.CantBeNull(Name);

            var rowId = await storage.AddRow(row);
            return CreatedAtAction(nameof(GetRow), new { id = rowId }, rowId);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> PutRow(string id, [FromBody] T row)
        {
            var _id = TryConvertToInt(id);
            if (_id is null) return error.IdInvalid(Name, id);
            
            if (row == null)
                return error.CantBeNull(Name);

            var updatedRow = await storage.UpdateRow((int)_id, row);
            if (!updatedRow)
                return error.IdNotFound(Name, id);

            return Ok(updatedRow);
        }


        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteRow(string id)
        {
            var _id = TryConvertToInt(id);
            if (_id is null) return error.IdInvalid(Name, id);

            var row = await storage.DeleteRow<T>((int)_id);
            if (!row)
                return error.IdNotFound(Name, id);

            return Ok($"{Name} deleted");
        }
    }
}
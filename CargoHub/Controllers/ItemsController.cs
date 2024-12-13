using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]    
    public class ItemsController(ItemStorageService storage) : BaseController<Item>(storage)
    {
        public override async Task<IActionResult> PostRow([FromBody] Item row)
        {
            if (row == null)
                return BadRequest("Item cannot be null");

            var rowId = await storage.AddRowUid(row);
            return CreatedAtAction(nameof(GetRow), new { id = rowId }, rowId);
        }
    }
}

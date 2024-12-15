using CargoHub.Models;
using CargoHub.Services;
using CargoHub.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]    
    public class ItemsController(ItemStorageService storage, ErrorHandler error) : BaseController<Item>(storage, error)
    {
        public override async Task<IActionResult> PostRow([FromBody] Item row)
        {
            if (row == null)
                return error.CantBeNull(base.Name);

            var rowId = await storage.AddRowUid(row);
            return CreatedAtAction(nameof(GetRow), new { id = rowId }, rowId);
        }
    }
}

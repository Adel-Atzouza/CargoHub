using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoHub.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsService _itemsService;

        public ItemsController(ItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetItems(string uid)
        // {
        //     // Fetch items from the service
        //     var items = await _itemsService.GetItems(uid);

        //     // If no items are found, return NotFound (404)
        //     if (items == null)
        //     {
        //         return NotFound("No items found matching the provided UID.");
        //     }

        //     // Return the list of items with a 200 OK response
        //     return Ok(items);
        // }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            // Fetch all items from the service
            var items = await _itemsService.GetAllItems();

            // If no items are found, return NotFound (404)
            if (items == null || items.Count == 0)
            {
                return NotFound("No items found.");
            }

            // Return the list of items with a 200 OK response
            return Ok(items);
        }


        [HttpGet("{uid}")]
        public async Task<IActionResult> GetItemByUid(string uid)
        {
            // Fetch the item from the service
            var item = await _itemsService.GetItem(uid);

            // If no item is found, return a 404 Not Found response
            if (item == null)
            {
                return NotFound("No item found with the provided UID.");
            }

            // If the item is found, return it with a 200 OK response
            return Ok(item);  // Ok() will wrap the item in an HTTP 200 response
        }

        [HttpPost]
        public async Task<IActionResult> PostItems([FromBody] Item newItem)
        {
            var createdItem = await _itemsService.PostItems(newItem);
            return CreatedAtAction(nameof(GetItemByUid), new { uid = createdItem.Uid }, createdItem);
        }

        [HttpDelete("{uid}")]
        public async Task<IActionResult> DeleteItemsByUid(string uid)
        {
            var result = await _itemsService.DeleteItemsByUid(uid);
            return Ok(result);
        }


        [HttpPut("{uid}")]
        public async Task<IActionResult> UpdateItem(string uid, [FromBody] Item updatedItem)
        {
            string result = await _itemsService.UpdateItem(uid, updatedItem);
            return Ok(result);
        }
    }
}
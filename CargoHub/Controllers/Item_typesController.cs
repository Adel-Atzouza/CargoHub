using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoHub.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemTypesController : ControllerBase
    {
        private readonly ItemTypesService _itemTypesService;

        public ItemTypesController(ItemTypesService itemTypesService)
        {
            _itemTypesService = itemTypesService;
        }

        // GET: /ItemTypes
        [HttpGet]
        public async Task<IActionResult> GetItemTypes()
        {
            // Fetch item types from the service
            var itemTypes = await _itemTypesService.GetItemTypes();

            // If no item types are found, return NotFound (404)
            if (itemTypes == null || itemTypes.Count == 0)
            {
                return NotFound("No item types found.");
            }

            // Return the list of item types with a 200 OK response
            return Ok(itemTypes);
        }

        // GET: /ItemTypes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemTypeById(int id)
        {
            // Fetch the item type from the service
            var itemType = await _itemTypesService.GetItemTypeById(id);

            // If no item type is found, return a 404 Not Found response
            if (itemType == null)
            {
                return NotFound("No item type found with the provided ID.");
            }

            // If the item type is found, return it with a 200 OK response
            return Ok(itemType);
        }

        // POST: /ItemTypes
        [HttpPost]
        public async Task<IActionResult> PostItemType([FromBody] Item_type newItemType)
        {
            // Create a new item type using the service
            var createdItemType = await _itemTypesService.PostItemType(newItemType);
            return CreatedAtAction(nameof(GetItemTypeById), new { id = createdItemType.Id }, createdItemType);
        }

        // DELETE: /ItemTypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemTypeById(int id)
        {
            var result = await _itemTypesService.DeleteItemTypeById(id);
            if (!result)
            {
                return NotFound("No item type found with the provided ID.");
            }
            return Ok("Item type deleted successfully.");
        }

        // PUT: /ItemTypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemType(int id, [FromBody] Item_type updatedItemType)
        {
            string result = await _itemTypesService.UpdateItemType(id, updatedItemType);
            if (result.Contains("Error"))
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}

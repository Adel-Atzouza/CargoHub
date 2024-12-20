using Microsoft.AspNetCore.Mvc;
using CargoHub.Services;
using CargoHub.Models;
namespace CargoHub.Controllers
{
    [Route("api/v1/[Controller]")]
    public class ItemGroupsController : ControllerBase
    {
        ItemGroupsService Service;
        public ItemGroupsController(ItemGroupsService service)
        {
            Service = service;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAll([FromQuery] int id = 1, [FromQuery] int PageSize = 100)
        {
            return Ok(await Service.GetAllItemGroups(id, PageSize));
        }
        [HttpGet()]
        public async Task<IActionResult> GetItemGroup([FromQuery] int id)
        {
            ItemGroup? Response = await Service.GetItemGroup(id);
            return Response is null ? NotFound($"The ItemGroup with Id {id} cannot be found") : Ok(Response);
        }
        [HttpPost()]
        public async Task<IActionResult> PostItemGroup([FromBody] ItemGroup itemGroup)
        {
            if (itemGroup == null || !ModelState.IsValid)
            {
                return BadRequest("Your input is invalid");
            }
            bool response = await Service.AddItemGroup(itemGroup);
            if (response)
            {
                string locationUri = $"/api/itemgroups/{itemGroup.Id}";
                var createdResponse = new
                {
                    Message = $"The item group with id {itemGroup.Id} has been added",
                    ItemGroup = itemGroup
                };

                return Created(locationUri, createdResponse);
            }

            return BadRequest("The item group that you're trying to add already exists");
        }

        [HttpPut()]
        public async Task<IActionResult> PutItemGroup([FromQuery] int id, [FromBody] ItemGroup Group)
        {
            bool response = await Service.UpdateItemGroup(id, Group);
            return response ? Ok("Item Group Updated") : BadRequest();
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteItemGroup([FromQuery] int id)
        {
            bool response = await Service.DeleteItemGroup(id);
            return response ? Ok($"Item Group with id: {id} has been deleted") : BadRequest();
        }

    }
}

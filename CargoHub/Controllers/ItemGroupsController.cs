using Microsoft.AspNetCore.Mvc;
namespace CargoHub
{
    [Route("api/v2/[Controller]")]
    [AuthorizationFilter]
    public class ItemGroupsController : ControllerBase
    {
        ItemGroupsService Service;
        public ItemGroupsController(ItemGroupsService service)
        {
            Service = service;
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
            bool response = await Service.AddItemGroup(itemGroup);
            return response ? Ok($"The item groups with id {itemGroup.Id} has been added")
                            : BadRequest("The item group that you're trying to add already exists");
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

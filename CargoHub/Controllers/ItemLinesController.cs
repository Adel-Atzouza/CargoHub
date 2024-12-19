using Microsoft.AspNetCore.Mvc;
using CargoHub.Services;
using CargoHub.Models;
namespace CargoHub.Controllers
{
    [Route("api/v1/[Controller]")]
    public class ItemLinesController : ControllerBase
    {
        ItemLinesService Service;
        public ItemLinesController(ItemLinesService service)
        {
            Service = service;
        }
        [HttpGet("GetMultiple")]
        public async Task<IActionResult> GetMultipleItemGroups([FromQuery] int[] Ids)
        {
            return Ok(await Service.GetMultipleItemLines(Ids));
        }
        [HttpGet()]
        public async Task<IActionResult> GetItemLine([FromQuery] int id)
        {
            ItemLine? Response = await Service.GetItemLine(id);
            return Response is null ? NotFound($"The ItemLine with Id {id} cannot be found") : Ok(Response);
        }
        [HttpPost()]
        public async Task<IActionResult> PostItemLine([FromBody] ItemLine Item_Line)
        {
            if (Item_Line == null || !ModelState.IsValid)
            {
                return BadRequest("Your input is invalid");
            }
            bool response = await Service.AddItemLine(Item_Line);
            if (response)
            {
                string locationUri = $"/api/ItemLine/{Item_Line.Id}";
                var createdResponse = new
                {
                    Message = $"The item group with id {Item_Line.Id} has been added",
                    ItemLine = Item_Line
                };

                return Created(locationUri, createdResponse);
            }

            return BadRequest("The item group that you're trying to add already exists");
        }
        [HttpPut]
        public async Task<IActionResult> PutItemLine([FromQuery] int id, [FromBody] ItemLine Line)
        {
            if (Line == null || !ModelState.IsValid)
            {
                return BadRequest("Your input is invalid");
            }
            bool response = await Service.UpdateItemLine(id, Line);
            return response ? Ok("ItemLine Updated") : BadRequest();
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteItemLine([FromQuery] int id)
        {
            bool response = await Service.DeleteItemLine(id);
            return response ? Ok($"Item Line with id: {id} has been deleted") : BadRequest();
        }

    }
}

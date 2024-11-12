using Microsoft.AspNetCore.Mvc;
namespace CargoHub
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
        public async Task<IActionResult> PostItemLine([FromBody] ItemLine itemLine)
        {
            bool response = await Service.AddItemLine(itemLine);
            return response ? Ok($"The item Lines with id {itemLine.Id} has been added")
                            : BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> PutItemLine([FromQuery] int id, [FromBody] ItemLine Line)
        {
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

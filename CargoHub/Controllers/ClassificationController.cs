using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
namespace CargoHub.Controllers
{
    [Route("api/v1/[Controller]")]
    public class ClassificationsController : ControllerBase
    {
        private ClassificationService _service;
        public ClassificationsController(ClassificationService service)
        {
            _service = service;
        }
        // [HttpGet()]
        // public async Task<IActionResult> GetAllClassifcations()
        // {
        //     List<Classification> classifications = await _service.GetAllClassifications();
        //     return Ok(classifications);
        // }
    }
}

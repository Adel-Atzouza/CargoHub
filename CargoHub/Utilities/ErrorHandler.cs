using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Utilities
{
    public class ErrorHandler : ControllerBase
    {
        public IActionResult IdNotFound(string name, string id)
        {
            return NotFound($"{name} doesn't exist with id: " + id);
        }

        public IActionResult IdInvalid(string name, string id)
        {
            return NotFound($"The id {id} is invalid for object {name}");
        }

        public IActionResult CantBeNull(string name)
        {
            return BadRequest($"{name} object can't be null");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using CargoHub.Services;

namespace CargoHub.Controllers
{
    [Route("api/v1/[Controller]")]
    public class MigrationsController : ControllerBase
    {
        private readonly MigrationsService MigrationService;

        public MigrationsController(MigrationsService Migrate)
        {
            MigrationService = Migrate;
        }

        [HttpPost()]
        public async Task<IActionResult> Migrate([FromQuery] string FolderName)
        {
            var files = MigrationService.ReadDataFolder(FolderName);
            return Ok(files);
        }
    }

}
using Microsoft.AspNetCore.Mvc;

namespace CargoHub
{
    [Route("api/v1/[Controller]")]
    public class MigrationsController : ControllerBase
    {
        private DataMigration Migration{ get; set; }
        public MigrationsController(DataMigration migration)
        {
            Migration = migration;
        }
        [HttpPost("Migrate")]
        public async Task<IActionResult> MigrateData()
        {
            var list = await Migration.MigrateData();
            return Ok(list);
        }
    }
}
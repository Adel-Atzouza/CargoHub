using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]
    public class TransfersController(BaseStorageService storage) : BaseController<Transfer>(storage)
    {
    }
}
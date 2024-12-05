using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]
    public class WarehousesController(BaseStorageService storage) : BaseController<Warehouse>(storage)
    {
    }
}
using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]
    public class SuppliersController(BaseStorageService storage) : BaseController<Supplier>(storage)
    {
    }
}